using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Application.Email;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scoring.Requests;
using AreaType = SmartValley.Domain.Entities.AreaType;

namespace SmartValley.WebApi.Scoring
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly IScoringManagerContractClient _scoringManagerContractClient;
        private readonly IScoringExpertsManagerContractClient _scoringExpertsManagerContractClient;
        private readonly MailService _mailService;
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IScoringManagerContractClient scoringManagerContractClient,
            IScoringExpertsManagerContractClient scoringExpertsManagerContractClient,
            MailService mailService,
            IUserRepository userRepository,
            IClock clock)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _scoringManagerContractClient = scoringManagerContractClient;
            _scoringExpertsManagerContractClient = scoringExpertsManagerContractClient;
            _mailService = mailService;
            _userRepository = userRepository;
            _clock = clock;
        }

        public async Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas)
        {
            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _scoringManagerContractClient.GetScoringAddressAsync(projectExternalId);

            var offerInfos = await _scoringExpertsManagerContractClient.GetOffersAsync(projectExternalId);
            var offersEndDate = offerInfos.Max(i => i.ExpirationTimestamp);

            if (offersEndDate == null)
                throw new AppErrorException(ErrorCode.OffersNotFound);

            var scoringId = await AddScoringAsync(project.Id, contractAddress, offersEndDate.Value);

            await AddAreasAsync(areas, scoringId);

            var expertAddresses = offerInfos.Select(o => (Address) o.ExpertAddress).Distinct().ToArray();
            var experts = await _userRepository.GetIdsByAddressesAsync(expertAddresses);

            var offers = await AddScoringOffersAsync(offerInfos, scoringId, experts);

            await NotifyExpertsAsync(offers, experts);
        }

        private async Task NotifyExpertsAsync(IReadOnlyCollection<ScoringOffer> offers, IReadOnlyCollection<User> experts)
        {
            var expertEmailsDictionary = experts.ToDictionary(e => e.Id, e => e.Email);
            foreach (var offer in offers)
                await _mailService.SendOfferEmailAsync(expertEmailsDictionary[offer.ExpertId]);
        }

        private async Task<ScoringOffer[]> AddScoringOffersAsync(
            IReadOnlyCollection<ScoringOfferInfo> offerInfos,
            long scoringId,
            IReadOnlyCollection<User> experts)
        {
            var expertsDictionary = experts.ToDictionary(e => e.Address);
            var offers = offerInfos
                         .Select(o => CreateOffer(scoringId, expertsDictionary[o.ExpertAddress].Id, o))
                         .ToArray();

            await _scoringOffersRepository.AddAsync(offers);

            return offers;
        }

        private async Task<long> AddScoringAsync(long projectId, string contractAddress, DateTimeOffset offersEndDate)
        {
            var scoring = new Domain.Entities.Scoring
                          {
                              ProjectId = projectId,
                              ContractAddress = contractAddress,
                              CreationDate = _clock.UtcNow,
                              OffersEndDate = offersEndDate
                          };

            await _scoringRepository.AddAsync(scoring);
            return scoring.Id;
        }

        private Task AddAreasAsync(IReadOnlyCollection<AreaRequest> areas, long scoringId)
        {
            var areaScorings = areas.Select(a => CreateAreaScoring(a, scoringId)).ToArray();
            return _scoringRepository.AddAreasAsync(areaScorings);
        }

        private static ScoringOffer CreateOffer(long scoringId, long expertId, ScoringOfferInfo offerInfo)
        {
            return new ScoringOffer
                   {
                       AreaId = offerInfo.Area,
                       ExpertId = expertId,
                       ScoringId = scoringId,
                       Status = ScoringOfferStatus.Pending,
                       ExpirationTimestamp = offerInfo.ExpirationTimestamp.Value
                   };
        }

        private static AreaScoring CreateAreaScoring(AreaRequest areaRequest, long scoringId)
        {
            return new AreaScoring
                   {
                       AreaId = areaRequest.Area.ToDomain(),
                       ScoringId = scoringId,
                       ExpertsCount = areaRequest.ExpertsCount
                   };
        }

        public async Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses)
        {
            var statistics = await _scoringRepository.GetIncompletedScoringAreaStatisticsAsync(_clock.UtcNow);

            var result = new List<ScoringProjectDetailsWithCounts>();
            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.Rejected))
            {
                var rejectedStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount + i.PendingCount);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.Rejected, rejectedStatistics));
            }

            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.InProgress))
            {
                var pendingStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount && i.RequiredCount < i.AcceptedCount + i.PendingCount);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.InProgress, pendingStatistics));
            }

            return result;
        }

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetPendingOfferDetailsAsync(long expertId)
            => _scoringOffersRepository.GetAllPendingByExpertAsync(expertId);

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetAcceptedOfferDetailsAsync(long expertId)
            => _scoringOffersRepository.GetAllAcceptedByExpertAsync(expertId);

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetExpertOffersHistoryAsync(long expertId, DateTimeOffset now)
            => _scoringOffersRepository.GetExpertOffersHistoryAsync(expertId, now);

        public Task AcceptOfferAsync(long scoringId, long areaId, long expertId)
        {
            return _scoringOffersRepository.AcceptAsync(scoringId, expertId, (AreaType) areaId);
        }

        public Task RejectOfferAsync(long scoringId, long areaId, long expertId)
        {
            return _scoringOffersRepository.RejectAsync(scoringId, expertId, (AreaType) areaId);
        }

        private async Task<IEnumerable<ScoringProjectDetailsWithCounts>> ConvertAreaStatisticsToProjectDetailsAsync(
            ScoringProjectStatus status,
            IEnumerable<ScoringAreaStatistics> statistics)
        {
            var areasByScoringId = statistics.ToLookup(
                o => o.ScoringId,
                k => new AreaCount
                     {
                         AreaType = k.AreaId,
                         AcceptedCount = k.AcceptedCount,
                         RequeiredCount = k.RequiredCount
                     });
            var scoringDetails = await _scoringRepository.GetScoringProjectsDetailsByScoringIdsAsync(areasByScoringId.Select(o => o.Key).ToArray());
            return scoringDetails.Select(i => new ScoringProjectDetailsWithCounts
                                              {
                                                  ProjectId = i.ProjectId,
                                                  CreationDate = i.CreationDate,
                                                  OffersEndDate = i.OffersEndDate,
                                                  Address = i.Address,
                                                  Name = i.Name,
                                                  Status = status,
                                                  AreaCounts = areasByScoringId[i.ScoringId]
                                              });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Email;
using SmartValley.Domain;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Projects;
using AreaType = SmartValley.Domain.Entities.AreaType;

namespace SmartValley.WebApi.Scorings
{
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly IScoringOffersManagerContractClient _scoringOffersManagerContractClient;
        private readonly MailService _mailService;
        private readonly IClock _clock;
        private readonly IScoringContractClient _scoringContractClient;
        private readonly IUserRepository _userRepository;
        private readonly IScoringsRegistryContractClient _scoringsRegistryContractClient;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringsRegistryContractClient scoringsRegistryContractClient,
            IScoringOffersRepository scoringOffersRepository,
            IScoringOffersManagerContractClient scoringOffersManagerContractClient,
            MailService mailService,
            IUserRepository userRepository,
            IClock clock,
            IScoringContractClient scoringContractClient)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _scoringOffersManagerContractClient = scoringOffersManagerContractClient;
            _mailService = mailService;
            _userRepository = userRepository;
            _clock = clock;
            _scoringsRegistryContractClient = scoringsRegistryContractClient;
            _scoringContractClient = scoringContractClient;
        }

        public async Task<ScoringOffer> GetOfferAsync(long projectId, AreaType areaType, long expertId)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            return scoring.GetOfferForExpertinArea(expertId, areaType);
        }

        public async Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses)
        {
            var statistics = await _scoringRepository.GetIncompletedScoringAreaStatisticsAsync(_clock.UtcNow);

            var result = new List<ScoringProjectDetailsWithCounts>();
            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.Rejected))
            {
                var rejectedStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount && !i.ScoringEndDate.HasValue && i.OffersEndDate < _clock.UtcNow);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.Rejected, rejectedStatistics));
            }

            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.InProgress))
            {
                var pendingStatistics = statistics.Where(i => i.RequiredCount > i.AcceptedCount && i.RequiredCount < i.AcceptedCount + i.PendingCount);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.InProgress, pendingStatistics));
            }

            if (!statuses.Any() || statuses.Contains(ScoringProjectStatus.All) || statuses.Contains(ScoringProjectStatus.AcceptedAndDoNotEstimate))
            {
                var acceptedAndDoNotEstimateStatistics = statistics.Where(i => i.AcceptedCount > i.FinishedCount && i.ScoringEndDate?.Date < _clock.UtcNow);
                result.AddRange(await ConvertAreaStatisticsToProjectDetailsAsync(ScoringProjectStatus.AcceptedAndDoNotEstimate, acceptedAndDoNotEstimateStatistics));

                return result;
            }

            return result;
        }

        public async Task AcceptOfferAsync(long scoringId, long areaId, long expertId)
        {
            var scoring = await _scoringRepository.GetByIdAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            var expert = await _userRepository.GetByIdAsync(expertId);
            var offer = await GetOfferFromContractAsync(areaId, scoring.ProjectId, expert);
            if (offer == null)
                throw new AppErrorException(ErrorCode.OfferNotFoundInContract);

            scoring.AcceptOffer(expertId, (AreaType) areaId, _clock.UtcNow);

            await _scoringRepository.SaveChangesAsync();
        }

        public async Task RejectOfferAsync(long scoringId, long areaId, long expertId)
        {
            var scoring = await _scoringRepository.GetByIdAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            scoring.RejectOffer(expertId, (AreaType) areaId);
            await _scoringRepository.SaveChangesAsync();
        }

        public async Task UpdateOffersAsync(Guid projectExternalId)
        {
            var scoringInfo = await _scoringOffersManagerContractClient.GetScoringInfoAsync(projectExternalId);
            var experts = await GetExpertsForOffersAsync(scoringInfo.Offers);
            var expertsDictionary = experts.ToDictionary(e => e.Address, e => e.Id);

            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var scoring = await _scoringRepository.GetByProjectIdAsync(project.Id);

            var removedOffers = scoring.ScoringOffers
                                       .Where(o => !scoringInfo.Offers.Any(e => expertsDictionary[e.ExpertAddress] == o.ExpertId && e.Area == o.AreaId))
                                       .ToArray();
            scoring.RemoveOffers(removedOffers);

            foreach (var offer in scoring.ScoringOffers)
            {
                var blockChainOffer = scoringInfo.Offers.FirstOrDefault(x => expertsDictionary[x.ExpertAddress] == offer.ExpertId && x.Area == offer.AreaId);
                if (blockChainOffer != null && blockChainOffer.Status != offer.Status)
                {
                    offer.Status = blockChainOffer.Status;
                }
            }

            var newOffers = scoringInfo.Offers
                                       .Where(o => !scoring.ScoringOffers.Any(e => e.AreaId == o.Area && e.ExpertId == expertsDictionary[o.ExpertAddress]))
                                       .Select(o => new ScoringOffer(expertsDictionary[o.ExpertAddress], o.Area, o.Status))
                                       .ToArray();
            
            if (newOffers.Any())
            {
                scoring.AddOffers(newOffers);
                await NotifyExpertsAsync(newOffers, experts);
            }

            var requiredExpertsCount = await _scoringsRegistryContractClient.GetRequiredExpertsCountsAsync(projectExternalId);
            scoring.UpdateExpertsCounts(requiredExpertsCount);

            scoring.AcceptingDeadline = scoringInfo.AcceptingDeadline;
            scoring.ScoringDeadline = scoringInfo.ScoringDeadline;

            await _scoringRepository.SaveChangesAsync();
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => _scoringRepository.GetByProjectIdAsync(projectId);

        public Task<PagingCollection<ScoringOfferDetails>> QueryOffersAsync(OffersQuery query, DateTimeOffset now)
            => _scoringOffersRepository.GetAsync(query, now);

        public async Task FinishAsync(long scoringId)
        {
            var scoring = await _scoringRepository.GetByIdAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            var project = await _projectRepository.GetByIdAsync(scoring.ProjectId);
            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            if (!project.IsPrivate)
                throw new AppErrorException(ErrorCode.ServerError, "'Finish' is allowed only for private scoring.");

            var scoringResults = await _scoringContractClient.GetResultsAsync(scoring.ContractAddress);

            foreach (var area in scoringResults.AreaScores.Keys)
            {
                if (scoring.HasEnoughEstimatesInArea(area))
                    scoring.SetScoreForArea(area, scoringResults.AreaScores[area]);
            }

            scoring.Finish(scoringResults.Score, _clock.UtcNow);
            await _scoringRepository.SaveChangesAsync();
        }

        public async Task ReopenAsync(long scoringId)
        {
            var scoring = await _scoringRepository.GetByIdAsync(scoringId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            var project = await _projectRepository.GetByIdAsync(scoring.ProjectId);
            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            if (!project.IsPrivate)
                throw new AppErrorException(ErrorCode.ServerError, "'Open' is allowed only for private scoring.");

            scoring.Reopen();
            await _scoringRepository.SaveChangesAsync();
        }

        private async Task<ScoringOfferInfo> GetOfferFromContractAsync(long areaId, long projectId, User expert)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var scoringInfo = await _scoringOffersManagerContractClient.GetScoringInfoAsync(project.ExternalId);
            return scoringInfo.Offers.FirstOrDefault(o => o.Area == (AreaType) areaId && o.ExpertAddress == expert.Address);
        }

        private Task<IReadOnlyCollection<User>> GetExpertsForOffersAsync(IReadOnlyCollection<ScoringOfferInfo> contractOffers)
        {
            var expertAddresses = contractOffers
                                  .Select(o => (Address) o.ExpertAddress)
                                  .Distinct()
                                  .ToArray();

            return _userRepository.GetByAddressesAsync(expertAddresses);
        }

        private Task NotifyExpertsAsync(IReadOnlyCollection<ScoringOffer> offers, IReadOnlyCollection<User> experts)
        {
            var expertEmailsDictionary = experts.ToDictionary(e => e.Id, e => e.Email);
            return Task.WhenAll(offers.Select(o => SendOfferEmailAsync(expertEmailsDictionary[o.ExpertId])));
        }

        private async Task SendOfferEmailAsync(string email)
        {
            try
            {
                await _mailService.SendOfferAsync(email);
            }
            catch (EmailSendingFailedException)
            {
                // TODO https://rassvet-capital.atlassian.net/browse/ILT-763
            }
        }

        private async Task<IEnumerable<ScoringProjectDetailsWithCounts>> ConvertAreaStatisticsToProjectDetailsAsync(
            ScoringProjectStatus status,
            IEnumerable<ScoringAreaStatistics> statistics)
        {
            var areasByScoringId = statistics.ToLookup(
                o => o.ScoringId,
                k => new AreaCount(k.AreaId, k.AcceptedCount, k.RequiredCount));

            var scoringDetails = await _scoringRepository.GetScoringProjectsDetailsByScoringIdsAsync(areasByScoringId.Select(o => o.Key).ToArray());
            return scoringDetails.Select(d => CreateScoringProjectDetailsWithCounts(status, areasByScoringId[d.ScoringId].ToArray(), d));
        }

        private static ScoringProjectDetailsWithCounts CreateScoringProjectDetailsWithCounts(
            ScoringProjectStatus status,
            IReadOnlyCollection<AreaCount> areaCounts,
            ScoringProjectDetails details)
        {
            return new ScoringProjectDetailsWithCounts(
                status,
                areaCounts,
                details.ProjectId,
                details.ProjectExternalId,
                details.ScoringId,
                details.Address,
                details.Name,
                details.CreationDate,
                details.OffersEndDate);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Application.Email;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Scoring.Requests;

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
        private readonly IUserRepository _userRepository;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IScoringManagerContractClient scoringManagerContractClient,
            IScoringExpertsManagerContractClient scoringExpertsManagerContractClient,
            MailService mailService,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _scoringManagerContractClient = scoringManagerContractClient;
            _scoringExpertsManagerContractClient = scoringExpertsManagerContractClient;
            _mailService = mailService;
            _userRepository = userRepository;
        }

        public async Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas)
        {
            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _scoringManagerContractClient.GetScoringAddressAsync(projectExternalId);

            var scoringId = await AddScoringAsync(project.Id, contractAddress);

            await AddAreasAsync(areas, scoringId);

            var offerInfos = await _scoringExpertsManagerContractClient.GetOffersAsync(projectExternalId);
            var expertAddresses = offerInfos.Select(o => o.ExpertAddress).Distinct().ToArray();
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

        private async Task<long> AddScoringAsync(long projectId, string contractAddress)
        {
            var scoring = new Domain.Entities.Scoring
                          {
                              ProjectId = projectId,
                              ContractAddress = contractAddress
                          };

            await _scoringRepository.AddAsync(scoring);
            return scoring.Id;
        }

        private async Task AddAreasAsync(IReadOnlyCollection<AreaRequest> areas, long scoringId)
        {
            var areaScorings = areas.Select(a => CreateAreaScoring(a, scoringId)).ToArray();
            await _scoringRepository.AddAreasAsync(areaScorings);
        }

        private static ScoringOffer CreateOffer(long scoringId, long expertId, ScoringOfferInfo offerInfo)
        {
            return new ScoringOffer
                   {
                       AreaId = offerInfo.Area,
                       ExpertId = expertId,
                       ScoringId = scoringId,
                       Status = ScoringOfferStatus.Pending,
                       Timestamp = offerInfo.Timestamp.Value
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
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
        private readonly IExpertRepository _expertRepository;
        private readonly IScoringManagerContractClient _scoringManagerContractClient;
        private readonly IScoringExpertsManagerContractClient _scoringExpertsManagerContractClient;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IExpertRepository expertRepository,
            IScoringManagerContractClient scoringManagerContractClient,
            IScoringExpertsManagerContractClient scoringExpertsManagerContractClient)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _expertRepository = expertRepository;
            _scoringManagerContractClient = scoringManagerContractClient;
            _scoringExpertsManagerContractClient = scoringExpertsManagerContractClient;
        }

        public async Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas)
        {
            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _scoringManagerContractClient.GetScoringAddressAsync(projectExternalId);

            var scoringId = await AddScoringAsync(project.Id, contractAddress);

            await AddAreasAsync(areas, scoringId);

            await AddOffersAsync(projectExternalId, scoringId);
        }

        private async Task AddOffersAsync(Guid projectExternalId, long scoringId)
        {
            var offerInfos = await _scoringExpertsManagerContractClient.GetOffersAsync(projectExternalId);
            var expertAddresses = offerInfos.Select(o => o.ExpertAddress).Distinct().ToArray();
            var expertIds = await _expertRepository.GetIdsByAddressesAsync(expertAddresses);
            var offers = offerInfos.Select(o => CreateOffer(scoringId, expertIds, o)).ToArray();
            await _scoringOffersRepository.AddAsync(offers);
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

        private static ScoringOffer CreateOffer(long scoringId, IDictionary<string, long> expertIds, ScoringOfferInfo offerInfo)
        {
            return new ScoringOffer
                   {
                       AreaId = offerInfo.Area,
                       ExpertId = expertIds[offerInfo.ExpertAddress],
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
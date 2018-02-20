using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Projects.Scoring.Requests;

namespace SmartValley.WebApi.Projects.Scoring
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IScoringManagerContractClient _scoringManagerContractClient;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringManagerContractClient scoringManagerContractClient)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringManagerContractClient = scoringManagerContractClient;
        }

        public async Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas)
        {
            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _scoringManagerContractClient.GetScoringAddressAsync(projectExternalId);
            var scoring = new Domain.Entities.Scoring
                          {
                              ProjectId = project.Id,
                              ContractAddress = contractAddress
                          };

            await _scoringRepository.AddAsync(scoring);

            var areaScorings = areas.Select(a => CreateAreaScoring(a, scoring.Id)).ToArray();
            await _scoringRepository.AddAreasAsync(areaScorings);
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
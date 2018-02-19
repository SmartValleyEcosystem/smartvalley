using System;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Domain.Interfaces;

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

        public async Task StartAsync(Guid projectExternalId)
        {
            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _scoringManagerContractClient.GetScoringAddressAsync(projectExternalId);
            var scoring = new Domain.Entities.Scoring
                          {
                              ProjectId = project.Id,
                              ContractAddress = contractAddress
                          };

            await _scoringRepository.AddAsync(scoring);
        }
    }
}
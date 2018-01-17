using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Application.Contracts;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Scoring
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IProjectManagerContractClient _projectManagerContractClient;
        private readonly EthereumClient _ethereumClient;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IProjectManagerContractClient projectManagerContractClient,
            EthereumClient ethereumClient)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _projectManagerContractClient = projectManagerContractClient;
            _ethereumClient = ethereumClient;
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress)
            => _projectRepository.GetForScoringAsync(expertAddress, expertiseArea);

        public Task<IReadOnlyCollection<ProjectScoring>> GetProjectsByAuthorAsync(string authorAddress)
            => _projectRepository.GetByAuthorAsync(authorAddress);

        public async Task StartAsync(Guid projectExternalId, string transactionHash)
        {
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);

            var project = await _projectRepository.GetByExternalIdAsync(projectExternalId);
            var contractAddress = await _projectManagerContractClient.GetProjectAddressAsync(projectExternalId.ToString());
            var scoring = new Domain.Entities.Scoring
                          {
                              ProjectId = project.Id,
                              ContractAddress = contractAddress
                          };

            await _scoringRepository.AddAsync(scoring);
        }
    }
}
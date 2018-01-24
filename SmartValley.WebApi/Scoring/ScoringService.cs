using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Application.Contracts.Scorings;
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
        private readonly IScoringManagerContractClient _scoringManagerContractClient;
        private readonly EthereumClient _ethereumClient;

        public ScoringService(
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IScoringManagerContractClient scoringManagerContractClient,
            EthereumClient ethereumClient)
        {
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _scoringManagerContractClient = scoringManagerContractClient;
            _ethereumClient = ethereumClient;
        }

        public async Task StartAsync(Guid projectExternalId, string transactionHash)
        {
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);

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
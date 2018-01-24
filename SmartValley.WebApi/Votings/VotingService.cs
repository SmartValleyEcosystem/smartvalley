using System;
using System.Linq;
using System.Threading.Tasks;
using IcoLab.Common;
using SmartValley.Application;
using SmartValley.Application.Contracts.Votings;
using SmartValley.Application.Contracts.Votings.Dto;
using SmartValley.Application.Exceptions;
using SmartValley.Application.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Votings
{
    public class VotingService : IVotingService
    {
        private readonly IVotingRepository _votingRepository;
        private readonly IVotingProjectRepository _votingProjectRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IVotingManagerContractClient _votingManagerContractClient;
        private readonly IVotingSprintContractClient _votingSprintContractClient;
        private readonly IDateTime _dateTime;
        private readonly EthereumClient _ethereumClient;

        public VotingService(IVotingRepository votingRepository,
                             IVotingManagerContractClient votingManagerContractClient,
                             IVotingSprintContractClient votingSprintContractClient,
                             IVotingProjectRepository votingProjectRepository,
                             IProjectRepository projectRepository,
                             IDateTime dateTime,
                             EthereumClient ethereumClient)
        {
            _votingRepository = votingRepository;
            _votingManagerContractClient = votingManagerContractClient;
            _votingSprintContractClient = votingSprintContractClient;
            _votingProjectRepository = votingProjectRepository;
            _projectRepository = projectRepository;
            _dateTime = dateTime;
            _ethereumClient = ethereumClient;
        }

        public Task<VotingSprintDetails> GetSprintAsync(string sprintAddress) => _votingSprintContractClient.GetDetailsAsync(sprintAddress);

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
            => _votingSprintContractClient.GetVoteAsync(sprintAddress, investorAddress, projectId);

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            if (string.IsNullOrEmpty(investorAddress))
                return null;
            return await _votingSprintContractClient.GetVotesAsync(sprintAddress, investorAddress);
        }

        public async Task<VotingSprintDetails> GetLastSprintDetailsAsync()
        {
            var lastSprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();
            if (lastSprintAddress.IsAddressEmpty())
                return null;
            return await _votingSprintContractClient.GetDetailsAsync(lastSprintAddress);
        }

        public async Task StartSprintAsync()
        {
            var lastSprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();
            if (lastSprintAddress != null)
            {
                var lastSprintEndDate = (await _votingSprintContractClient.GetDetailsAsync(lastSprintAddress)).EndDate;
                if (_dateTime.UtcNow < lastSprintEndDate)
                    throw new AppErrorException(ErrorCode.VotingSprintAlreadyInProgress);
            }

            var projectsQueue = await _votingManagerContractClient.GetProjectsQueueAsync();
            if (projectsQueue.Count <= 1)
            {
                throw new AppErrorException(ErrorCode.NotEnoughProjectsForSprintStart);
            }

            var newVoting = await CreateVotingAsync();
            await _votingRepository.AddAsync(newVoting);

            var projects = await _projectRepository.GetByExternalIdsAsync(projectsQueue.Select(p => p).ToArray());

            await _votingProjectRepository.AddRangeAsync(projects.Select(p => new VotingProject
                                                                              {
                                                                                  ProjectId = p.Id,
                                                                                  VotingId = newVoting.Id
                                                                              }));
        }

        private async Task<Voting> CreateVotingAsync()
        {
            var txHash = await _votingManagerContractClient.CreateSprintAsync();
            await _ethereumClient.WaitForConfirmationAsync(txHash);

            var sprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();
            var sprint = await _votingSprintContractClient.GetDetailsAsync(sprintAddress);
            var newVoting = new Voting
                            {
                                StartDate = sprint.StartDate,
                                EndDate = sprint.EndDate,
                                VotingAddress = sprintAddress
                            };
            return newVoting;
        }
    }
}
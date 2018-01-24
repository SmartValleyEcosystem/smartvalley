using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IcoLab.Common;
using SmartValley.Application;
using SmartValley.Application.Contracts.Votings;
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

        public Task<VotingSprintDetails> GetSprintAsync(string sprintAddress)
            => _votingSprintContractClient.GetDetailsAsync(sprintAddress);

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
            => _votingSprintContractClient.GetVoteAsync(sprintAddress, investorAddress, projectId);

        public Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
            => _votingSprintContractClient.GetVotesAsync(sprintAddress, investorAddress);

        public async Task<VotingSprintDetails> GetLastSprintDetailsAsync()
        {
            var lastSprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();

            if (lastSprintAddress.IsAddressEmpty())
                return null;

            return await _votingSprintContractClient.GetDetailsAsync(lastSprintAddress);
        }

        public async Task StartSprintAsync()
        {
            if (await IsVotingInProgressAsync())
                throw new AppErrorException(ErrorCode.VotingSprintAlreadyInProgress);

            var projectsQueue = await _votingManagerContractClient.GetProjectsQueueAsync();
            var minimumProjectsCount = await _votingManagerContractClient.GetMinimumVotingProjectsCountAsync();
            if (projectsQueue.Count < minimumProjectsCount)
                throw new AppErrorException(ErrorCode.NotEnoughProjectsForSprintStart);

            await CreateVotingAsync(projectsQueue);
        }

        public async Task<VotingProjectDetails> GetVotingProjectDetailsAsync(long projectId)
        {
            var votingProject = await _votingProjectRepository.GetByProjectAsync(projectId);
            if (votingProject == null)
                return null;

            var project = await _projectRepository.GetByIdAsync(projectId);
            var voting = await _votingRepository.GetByIdAsync(votingProject.VotingId);
            var isAccepted = await _votingSprintContractClient.IsAcceptedAsync(voting.VotingAddress, project.ExternalId);
            return new VotingProjectDetails(projectId, voting, isAccepted);
        }

        private async Task CreateVotingAsync(IReadOnlyCollection<Guid> projectExternalIds)
        {
            var transactionHash = await _votingManagerContractClient.CreateSprintAsync();
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);

            var sprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();
            var votingDetails = await _votingSprintContractClient.GetDetailsAsync(sprintAddress);

            var votingId = await SaveVotingAsync(votingDetails, sprintAddress);
            await SaveVotingProjectsAsync(projectExternalIds, votingId);
        }

        private async Task<long> SaveVotingAsync(
            VotingSprintDetails votingDetails,
            string sprintAddress)
        {
            var voting = new Voting
                         {
                             StartDate = votingDetails.StartDate,
                             EndDate = votingDetails.EndDate,
                             VotingAddress = sprintAddress
                         };
            await _votingRepository.AddAsync(voting);
            return voting.Id;
        }

        private async Task SaveVotingProjectsAsync(IReadOnlyCollection<Guid> projectExternalIds, long votingId)
        {
            var projects = await _projectRepository.GetByExternalIdsAsync(projectExternalIds);
            var votingProjects = projects.Select(p => new VotingProject {ProjectId = p.Id, VotingId = votingId});
            await _votingProjectRepository.AddRangeAsync(votingProjects);
        }

        private async Task<bool> IsVotingInProgressAsync()
        {
            var lastSprintAddress = await _votingManagerContractClient.GetLastSprintAddressAsync();
            if (lastSprintAddress == null)
                return false;

            var lastSprintDetails = await _votingSprintContractClient.GetDetailsAsync(lastSprintAddress);
            return _dateTime.UtcNow < lastSprintDetails.EndDate;
        }
    }
}
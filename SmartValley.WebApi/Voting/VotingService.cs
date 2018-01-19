using System;
using System.Threading.Tasks;
using SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.VotingSprint;
using SmartValley.Application.Extensions;
using SmartValley.Domain;

namespace SmartValley.WebApi.Voting
{
    public class VotingService : IVotingService
    {
        private readonly IVotingManagerContractClient _votingManagetClient;
        private readonly IVotingSprintContractClient _votingSprintClient;

        public VotingService(IVotingManagerContractClient votingManagetClient, IVotingSprintContractClient votingSprintClient)
        {
            _votingManagetClient = votingManagetClient;
            _votingSprintClient = votingSprintClient;
        }

        public async Task<Sprint> GetSprintAsync(string sprintAddress) => await _votingSprintClient.GetSprintByAddress(sprintAddress);

        public async Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
            => await _votingSprintClient.GetVoteAsync(sprintAddress, investorAddress, projectId);

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
            => await _votingSprintClient.GetVotesAsync(sprintAddress, investorAddress);

        public async Task<Sprint> GetLastSprintAsync()
        {
            var lastSprintAddress = await _votingManagetClient.GetLastSprintAddressAsync();
            if (lastSprintAddress.IsAddressEmpty())
                return null;
            return await GetSprintAsync(lastSprintAddress);
        }
    }
}

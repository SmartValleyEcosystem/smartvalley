using System;
using System.Threading.Tasks;
using SmartValley.Domain;

namespace SmartValley.Application.Contracts.VotingSprint
{
    public interface IVotingSprintContractClient
    {
        Task<Sprint> GetSprintByAddress(string sprintAddress);
        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);
        Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);
    }
}

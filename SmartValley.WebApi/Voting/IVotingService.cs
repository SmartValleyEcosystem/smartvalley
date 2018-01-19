using System;
using System.Threading.Tasks;
using SmartValley.Domain;

namespace SmartValley.WebApi.Voting
{
    public interface IVotingService
    {
        Task<Sprint> GetSprintAsync(string sprintAddress);
        Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);
        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);
        Task<Sprint> GetLastSprintAsync();
    }
}

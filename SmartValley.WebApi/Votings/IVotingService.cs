using System;
using System.Threading.Tasks;
using SmartValley.Domain;

namespace SmartValley.WebApi.Votings
{
    public interface IVotingService
    {
        Task<VotingSprintDetails> GetSprintAsync(string sprintAddress);

        Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);

        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);

        Task<VotingSprintDetails> GetLastSprintDetailsAsync();

        Task StartSprintAsync();

        Task<VotingProjectDetails> GetVotingProjectDetailsAsync(long projectId);
    }
}
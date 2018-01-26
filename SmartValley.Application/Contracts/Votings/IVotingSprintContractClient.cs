using System;
using System.Threading.Tasks;
using SmartValley.Domain;
using System.Numerics;

namespace SmartValley.Application.Contracts.Votings
{
    public interface IVotingSprintContractClient
    {
        Task<VotingSprintDetails> GetDetailsAsync(string sprintAddress);

        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);

        Task<double> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);

        Task<bool> IsAcceptedAsync(string sprintAddress, Guid projectId);

        Task<double> GetProjectTotalTokensAsync(string sprintAddress, Guid projectExternalId);
    }
}
using System;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Votings.Dto;
using SmartValley.Domain;

namespace SmartValley.Application.Contracts.Votings
{
    public interface IVotingSprintContractClient
    {
        Task<VotingSprintDetails> GetDetailsAsync(string sprintAddress);

        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);

        Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);

        Task<bool> IsAcceptedAsync(string sprintAddress, Guid projectId);
    }
}
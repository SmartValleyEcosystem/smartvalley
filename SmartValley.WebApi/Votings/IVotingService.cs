using System;
using System.Threading.Tasks;
using SmartValley.Domain;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Votings
{
    public interface IVotingService
    {
        Task<VotingSprintDetails> GetSprintDetailsByAddressAsync(string address);

        Task<InvestorVotesDetails> GetVotesAsync(string sprintAddress, IReadOnlyCollection<Guid> sprintProjectExternalIds, string investorAddress);

        Task<VotingSprintDetails> GetLastSprintDetailsAsync();

        Task StartSprintAsync();

        Task<IReadOnlyCollection<Voting>> GetCompletedSprintsAsync();

        Task<VotingProjectDetails> GetVotingProjectDetailsAsync(long projectId);
    }
}
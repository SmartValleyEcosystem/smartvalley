using System;
using System.Threading.Tasks;
using SmartValley.Domain;
using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Votings
{
    public interface IVotingService
    {
        Task<VotingSprintDetails> GetSprintDetailsByAddressAsync(Address address);

        Task<InvestorVotesDetails> GetVotesAsync(Address sprintAddress, IReadOnlyCollection<Guid> sprintProjectExternalIds, Address investorAddress);

        Task<VotingSprintDetails> GetLastSprintDetailsAsync();

        Task StartSprintAsync();

        Task<IReadOnlyCollection<Voting>> GetCompletedSprintsAsync();

        Task<VotingProjectDetails> GetVotingProjectDetailsAsync(long projectId);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Application.Contracts.Votings
{
    public interface IVotingManagerContractClient
    {
        Task<IReadOnlyCollection<Guid>> GetProjectsQueueAsync();

        Task<string> CreateSprintAsync();

        Task<string> GetLastSprintAddressAsync();

        Task<uint> GetMinimumVotingProjectsCountAsync();
    }
}
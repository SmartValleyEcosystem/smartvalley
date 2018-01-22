using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IVotingProjectRepository
    {
        Task AddRangeAsync(IEnumerable<VotingProject> votingProjects);
    }
}
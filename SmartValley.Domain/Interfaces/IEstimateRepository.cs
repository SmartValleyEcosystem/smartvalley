using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEstimateRepository
    {
        Task<int> AddRangeAsync(IEnumerable<Estimate> entities);

        Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId);

        Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ScoringCategory category);

        Task<IReadOnlyCollection<long>> GetProjectsEstimatedByExpertAsync(string expertAddress, ScoringCategory category);
    }
}
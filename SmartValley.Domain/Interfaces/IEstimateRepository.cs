using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEstimateRepository
    {
        Task<int> AddRangeAsync(IEnumerable<Estimate> entities);

        Task<IReadOnlyCollection<Estimate>> GetByProjectAsync(long projectId);
        Task<IReadOnlyCollection<Estimate>> GetByProjectIdAndCategoryAsync(long projectId, ScoringCategory category);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEstimateRepository
    {
        Task<int> AddRangeAsync(IEnumerable<EstimateComment> entities);

        Task AddConclusionAsync(long expertId, long scoringId, AreaType area, string conclusion);

        Task<IReadOnlyCollection<EstimateComment>> GetByScoringIdAsync(long scoringId, AreaType areaType);
    }
}
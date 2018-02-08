using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEstimateCommentRepository
    {
        Task<int> AddRangeAsync(IEnumerable<EstimateComment> entities);

        Task<IReadOnlyCollection<EstimateComment>> GetAsync(long projectId, ExpertiseAreaType expertiseAreaType);
    }
}
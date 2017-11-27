using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);
        Task<IReadOnlyCollection<Estimate>> GetByProjectIdAndCategory(long projectId, Category category);
    }
}
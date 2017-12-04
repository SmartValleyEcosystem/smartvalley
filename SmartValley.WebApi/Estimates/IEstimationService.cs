using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);
        Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ExpertiseAreaApi expertiseAreaApi);
        double CalculateAverageScore(IReadOnlyCollection<Estimate> estimates);
    }
}
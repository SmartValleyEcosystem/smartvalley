using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);
        Task<Dictionary<long, IReadOnlyCollection<Estimate>>> GetQuestionWithEstimatesAsync(long projectId, ExpertiseArea expertiseArea);
        double CalculateAverageScore(IReadOnlyCollection<Estimate> estimates);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.WebApi.Estimates.Requests;

namespace SmartValley.WebApi.Estimates
{       
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(long expertId, SubmitEstimatesRequest request);

        Task<IReadOnlyCollection<ScoringStatisticsInArea>> GetScoringStatisticsAsync(long projectId);

        Task<IReadOnlyCollection<ScoringCriterionPrompt>> GetCriterionPromptsAsync(long projectId, Domain.Entities.AreaType areaType);
    }
}
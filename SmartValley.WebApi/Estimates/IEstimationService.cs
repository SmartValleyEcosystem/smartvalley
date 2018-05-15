using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Estimates.Requests;

namespace SmartValley.WebApi.Estimates
{       
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(long expertId, SubmitEstimateRequest request);

        Task<IReadOnlyCollection<ScoringStatisticsInArea>> GetScoringStatisticsAsync(long projectId);

        Task<IReadOnlyCollection<ScoringCriterionPrompt>> GetCriterionPromptsAsync(long projectId, AreaType areaType);

        Task SaveEstimatesAsync(long expertId, SaveEstimatesRequest request);

        Task<ExpertScoring> GetOfferEstimateAsync(long expertId, long projectId, Experts.AreaType areaType);
    }
}
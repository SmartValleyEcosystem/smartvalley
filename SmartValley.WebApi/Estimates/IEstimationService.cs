using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.WebApi.Estimates.Requests;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);

        Task<ScoringStatisticsInArea> GetScoringStatisticsInAreaAsync(long projectId, Domain.Entities.ExpertiseArea expertiseArea);
    }
}
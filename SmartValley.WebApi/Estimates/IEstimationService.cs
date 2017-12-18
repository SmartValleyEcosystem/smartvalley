using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);

        Task<Dictionary<long, IReadOnlyCollection<Estimate>>> GetQuestionsWithEstimatesAsync(long projectId, string projectAddress, Domain.Entities.ExpertiseArea expertiseArea);
    }
}
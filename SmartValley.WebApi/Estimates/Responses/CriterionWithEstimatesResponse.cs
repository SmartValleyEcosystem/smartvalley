using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class CriterionWithEstimatesResponse
    {
        public long ScoringCriterionId { get; set; }

        public IReadOnlyCollection<EstimateResponse> Estimates { get; set; }

        public static CriterionWithEstimatesResponse Create(long scoringCriterionId, IReadOnlyCollection<Estimate> estimates)
        {
            return new CriterionWithEstimatesResponse
                   {
                       ScoringCriterionId = scoringCriterionId,
                       Estimates = estimates.Select(EstimateResponse.Create).ToArray()
                   };
        }
    }
}
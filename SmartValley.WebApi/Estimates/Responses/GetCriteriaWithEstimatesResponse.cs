using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class GetCriteriaWithEstimatesResponse
    {
        public double? Score { get; set; }

        public IReadOnlyCollection<CriterionWithEstimatesResponse> Criteria { get; set; }

        public static GetCriteriaWithEstimatesResponse Create(ScoringStatisticsInArea scoringStatisticsInArea)
        {
            return new GetCriteriaWithEstimatesResponse
                   {
                       Score = scoringStatisticsInArea.Score,
                       Criteria = scoringStatisticsInArea
                           .Estimates
                           .GroupBy(e => e.ScoringCriterionId)
                           .Select(q => CriterionWithEstimatesResponse.Create(q.Key, q.ToArray()))
                           .ToArray()
                   };
        }
    }
}
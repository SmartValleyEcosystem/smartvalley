using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.WebApi.Scorings.Responses;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringStatisticsInAreaResponse
    {
        public double? Score { get; set; }

        public long RequiredExpertsCount { get; set; }

        public IEnumerable<ScoringAreaConslusionResponse> Conclusions { get; set; }

        public IReadOnlyCollection<CriterionWithEstimatesResponse> Criteria { get; set; }

        public IReadOnlyCollection<ScoringOfferAreaResponse> Offers { get; set; }

        public static ScoringStatisticsInAreaResponse Create(ScoringStatisticsInArea scoringStatisticsInArea)
        {
            return new ScoringStatisticsInAreaResponse
                   {
                       Score = scoringStatisticsInArea.Score,
                       RequiredExpertsCount = scoringStatisticsInArea.RequiredExpertsCount,
                       Conclusions = scoringStatisticsInArea.Conclusions.Select(ScoringAreaConslusionResponse.FromDomain),
                       Offers = scoringStatisticsInArea.Offers.Select(ScoringOfferAreaResponse.FromDomain).ToArray(),
                       Criteria = scoringStatisticsInArea
                           .Estimates
                           .GroupBy(e => e.ScoringCriterionId)
                           .Select(q => CriterionWithEstimatesResponse.Create(q.Key, q.ToArray()))
                           .ToArray()
                   };
        }
    }
}
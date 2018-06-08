using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Scorings.Responses;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringReportInAreaResponse
    {
        public double? Score { get; set; }

        public long RequiredExpertsCount { get; set; }

        public IEnumerable<ScoringAreaConslusionResponse> Conclusions { get; set; }

        public IReadOnlyCollection<CriterionWithEstimatesResponse> Criteria { get; set; }

        public IReadOnlyCollection<ScoringOfferAreaResponse> Offers { get; set; }

        public AreaType AreaType { get; set; }

        public static ScoringReportInAreaResponse Create(ScoringReportInArea scoringReportInArea, DateTimeOffset acceptingDeadline, DateTimeOffset scoringDeadline, DateTimeOffset now)
        {
            return new ScoringReportInAreaResponse
                   {
                       Score = scoringReportInArea.Score,
                       RequiredExpertsCount = scoringReportInArea.RequiredExpertsCount,
                       Conclusions = scoringReportInArea.Scorings.Select(ScoringAreaConslusionResponse.FromDomain),
                       Offers = scoringReportInArea.Offers.Select(x => ScoringOfferAreaResponse.FromDomain(x, acceptingDeadline, scoringDeadline, now)).ToArray(),
                       Criteria = scoringReportInArea
                                  .Scorings
                                  .SelectMany(x => x.Estimates)
                                  .GroupBy(e => e.ScoringCriterionId)
                                  .Select(q => CriterionWithEstimatesResponse.Create(q.Key, q.ToArray()))
                                  .ToArray(),
                       AreaType = scoringReportInArea.AreaType.FromDomain()
                   };
        }
    }
}
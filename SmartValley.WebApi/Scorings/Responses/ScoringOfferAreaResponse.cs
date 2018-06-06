using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings.Responses
{
    public class ScoringOfferAreaResponse
    {
        public AreaType Area { get; set; }

        public ScoringOfferStatus Status { get; set; }

        public static ScoringOfferAreaResponse FromDomain(ScoringOffer scoringOffer, DateTimeOffset acceptingDeadline, DateTimeOffset scoringDeadline, DateTimeOffset now)
        {
            return new ScoringOfferAreaResponse
                   {
                       Area = scoringOffer.AreaId,
                       Status = scoringOffer.Status.ToApi(acceptingDeadline, scoringDeadline, now)
                   };
        }
    }
}
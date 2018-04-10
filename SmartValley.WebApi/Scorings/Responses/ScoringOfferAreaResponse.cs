using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings.Responses
{
    public class ScoringOfferAreaResponse
    {
        public AreaType Area { get; set; }

        public Domain.Entities.ScoringOfferStatus Status { get; set; }

        public static ScoringOfferAreaResponse FromDomain(ScoringOffer scoringOffer)
        {
            return new ScoringOfferAreaResponse
                   {
                       Area = scoringOffer.AreaId,
                       Status = scoringOffer.Status
                   };
        }
    }
}
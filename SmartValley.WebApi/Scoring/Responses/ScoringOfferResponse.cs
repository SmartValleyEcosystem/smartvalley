using System;
using SmartValley.Domain;

namespace SmartValley.WebApi.Scoring.Responses
{
    public class ScoringOfferResponse
    {
        public long ScoringId { get; set; }

        public long AreaId { get; set; }

        public string Name { get; set; }

        public string ScoringContractAddress { get; set; }

        public string Country { get; set; }

        public string ProjectArea { get; set; }

        public string Description { get; set; }

        public Guid ProjectExternalId { get; set; }

        public DateTimeOffset ScoringOfferTimestamp { get; set; }

        public static ScoringOfferResponse Create(ScoringOfferDetails scoringOffer)
        {
            return new ScoringOfferResponse
                   {
                       ScoringContractAddress = scoringOffer.ScoringContractAddress,
                       ProjectArea = scoringOffer.ProjectArea,
                       Name = scoringOffer.Name,
                       Description = scoringOffer.Description,
                       AreaId = (long) scoringOffer.AreaType,
                       Country = scoringOffer.Country,
                       ScoringId = scoringOffer.ScoringId,
                       ProjectExternalId = scoringOffer.ProjectExternalId,
                       ScoringOfferTimestamp = scoringOffer.ScoringOfferTimestamp
                   };
        }
    }
}
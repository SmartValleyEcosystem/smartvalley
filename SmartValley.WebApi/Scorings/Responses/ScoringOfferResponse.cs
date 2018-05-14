using System;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings.Responses
{
    public class ScoringOfferResponse
    {
        public long ScoringId { get; set; }

        public long ProjectId { get; set; }

        public long Area { get; set; }

        public string Name { get; set; }

        public string ScoringContractAddress { get; set; }

        public string CountryCode { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }

        public ScoringOfferStatus OfferStatus { get; set; }

        public Guid ProjectExternalId { get; set; }

        public DateTimeOffset? ExpirationTimestamp { get; set; }

        public DateTimeOffset? EstimatesDueDate { get; set; }

        public bool IsPrivate { get; set; }

        public double? FinalScore { get; set; }

        public static ScoringOfferResponse Create(ScoringOfferDetails scoringOffer, DateTimeOffset now)
        {
            return new ScoringOfferResponse
                   {
                       ScoringContractAddress = scoringOffer.ScoringContractAddress,
                       Category = scoringOffer.Category,
                       Name = scoringOffer.Name,
                       Description = scoringOffer.Description,
                       Area = (long) scoringOffer.AreaType,
                       CountryCode = scoringOffer.CountryCode,
                       ScoringId = scoringOffer.ScoringId,
                       ProjectExternalId = scoringOffer.ProjectExternalId,
                       ExpirationTimestamp = scoringOffer.ExpirationTimestamp,
                       EstimatesDueDate = scoringOffer.EstimatesDueDate,
                       IsPrivate = scoringOffer.IsPrivate,
                       OfferStatus = scoringOffer.Status.ToApi(scoringOffer.ExpirationTimestamp, now),
                       ProjectId = scoringOffer.ProjectId,
                       FinalScore = scoringOffer.FinalScore
                   };
        }
    }
}
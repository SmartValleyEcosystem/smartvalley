using System;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

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

        public OfferStatus OfferStatus { get; set; }

        public Guid ProjectExternalId { get; set; }

        public DateTimeOffset ScoringOfferTimestamp { get; set; }

        public static ScoringOfferResponse Create(ScoringOfferDetails scoringOffer, DateTimeOffset now)
        {
            return new ScoringOfferResponse
                   {
                       ScoringContractAddress = scoringOffer.ScoringContractAddress,
                       ProjectArea = scoringOffer.ProjectArea,
                       Name = scoringOffer.Name,
                       Description = scoringOffer.Description,
                       AreaId = (long) scoringOffer.AreaType,
                       Country = scoringOffer.CountryCode,
                       ScoringId = scoringOffer.ScoringId,
                       ProjectExternalId = scoringOffer.ProjectExternalId,
                       ScoringOfferTimestamp = scoringOffer.ScoringOfferTimestamp,
                       OfferStatus = GetStatus(scoringOffer, now)
                   };
        }

        private static OfferStatus GetStatus(ScoringOfferDetails scoringOffer, DateTimeOffset now)
        {
            if (scoringOffer == null)
                return OfferStatus.None;

            switch (scoringOffer.ScoringOfferStatus)
            {
                case ScoringOfferStatus.Pending:
                    return scoringOffer.ScoringOfferTimestamp < now ? OfferStatus.Timeout : OfferStatus.Pending;
                case ScoringOfferStatus.Accepted:
                    return OfferStatus.Accepted;
                case ScoringOfferStatus.Rejected:
                    return OfferStatus.Rejected;
                case ScoringOfferStatus.Finished:
                    return OfferStatus.Finished;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public class ScoringOfferInfo
    {
        public ScoringOfferInfo(
            Guid projectExternalId,
            string expertAddress,
            AreaType area,
            ScoringOfferStatus status,
            DateTimeOffset? expirationTimestamp)
        {
            ProjectExternalId = projectExternalId;
            ExpertAddress = expertAddress;
            Area = area;
            Status = status;
            ExpirationTimestamp = expirationTimestamp;
        }

        public Guid ProjectExternalId { get; }

        public string ExpertAddress { get; }

        public AreaType Area { get; }

        public ScoringOfferStatus Status { get; }

        public DateTimeOffset? ExpirationTimestamp { get; }
    }
}
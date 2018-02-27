using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Scorings
{
    public class ScoringOfferInfo
    {
        public ScoringOfferInfo(Guid projectExternalId, string expertAddress, AreaType area, ScoringOfferStatus status, DateTimeOffset? timestamp)
        {
            ProjectExternalId = projectExternalId;
            ExpertAddress = expertAddress;
            Area = area;
            Status = status;
            Timestamp = timestamp;
        }

        public Guid ProjectExternalId { get; }

        public string ExpertAddress { get; }

        public AreaType Area { get; }

        public ScoringOfferStatus Status { get; }

        public DateTimeOffset? Timestamp { get; }
    }
}
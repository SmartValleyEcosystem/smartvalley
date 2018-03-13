using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringAreaStatistics
    {
        public AreaType AreaId { get; set; }

        public long ScoringId { get; set; }

        public int RequiredCount { get; set; }

        public int AcceptedCount { get; set; }

        public int PendingCount { get; set; }

        public int FinishedCount { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public DateTimeOffset OffersEndDate { get; set; }
    }
}

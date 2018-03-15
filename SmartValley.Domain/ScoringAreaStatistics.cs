using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringAreaStatistics
    {
        public AreaType AreaId { get; }

        public long ScoringId { get; }

        public int RequiredCount { get; }

        public int AcceptedCount { get; }

        public int PendingCount { get; }

        public int FinishedCount { get; }

        public DateTimeOffset? ScoringEndDate { get; }

        public DateTimeOffset OffersEndDate { get; }

        public ScoringAreaStatistics(
            AreaType areaId, 
            long scoringId, 
            int requiredCount, 
            int acceptedCount, 
            int pendingCount, 
            int finishedCount, 
            DateTimeOffset? scoringEndDate, 
            DateTimeOffset offersEndDate)
        {
            AreaId = areaId;
            ScoringId = scoringId;
            RequiredCount = requiredCount;
            AcceptedCount = acceptedCount;
            PendingCount = pendingCount;
            FinishedCount = finishedCount;
            ScoringEndDate = scoringEndDate;
            OffersEndDate = offersEndDate;
        }
    }
}
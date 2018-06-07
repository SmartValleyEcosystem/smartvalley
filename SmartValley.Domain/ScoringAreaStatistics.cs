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

        public DateTimeOffset ScoringDeadline { get; }

        public DateTimeOffset AcceptingDeadline { get; }

        public ScoringAreaStatistics(AreaType areaId,
                                     long scoringId,
                                     int requiredCount,
                                     int acceptedCount,
                                     int pendingCount,
                                     DateTimeOffset scoringDeadline,
                                     DateTimeOffset acceptingDeadline)
        {
            AreaId = areaId;
            ScoringId = scoringId;
            RequiredCount = requiredCount;
            AcceptedCount = acceptedCount;
            PendingCount = pendingCount;
            ScoringDeadline = scoringDeadline;
            AcceptingDeadline = acceptingDeadline;
        }

        public bool HasAcceptingPhaseTimedOut(DateTimeOffset now)
            => IsAcceptingPhaseInProgress() && (TooManyRejections() || AcceptingDeadline < now);

        public bool HasScoringPhaseTimedOut(DateTimeOffset now)
            => IsScoringPhaseInProgress() && ScoringDeadline < now;

        private bool IsScoringPhaseInProgress()
            => RequiredCount == AcceptedCount;

        private bool TooManyRejections()
            => RequiredCount > AcceptedCount + PendingCount;

        private bool IsAcceptingPhaseInProgress()
            => RequiredCount > AcceptedCount;
    }
}
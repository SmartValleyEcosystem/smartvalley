using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class OffersQuery
    {
        public OffersQuery(int offset,
                           int count,
                           IReadOnlyCollection<ScoringOfferStatus> statuses = null,
                           long? expertId = null,
                           long? scoringId = null,
                           long? projectId = null,
                           ScoringOffersOrderBy? orderBy = null,
                           SortDirection? sortDirection = null)
        {
            Offset = offset;
            Count = count;
            ExpertId = expertId;
            ScoringId = scoringId;
            ProjectId = projectId;
            Statuses = statuses ?? new ScoringOfferStatus[0];
            OrderBy = orderBy;
            SortDirection = sortDirection;
        }

        public int Offset { get; }

        public int Count { get; }

        public long? ExpertId { get; }

        public long? ScoringId { get; }

        public long? ProjectId { get; }

        public IReadOnlyCollection<ScoringOfferStatus> Statuses { get; }

        public ScoringOffersOrderBy? OrderBy { get; }

        public SortDirection? SortDirection { get; }
    }
}
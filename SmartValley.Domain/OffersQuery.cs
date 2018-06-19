using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class OffersQuery
    {
        public int Offset { get; set; }

        public int Count { get; set; }

        public long? ExpertId { get; set; }

        public long? ScoringId { get; set; }

        public long? ProjectId { get; set; }

        public IReadOnlyCollection<ScoringOfferStatus> Statuses { get; set; }

        public ScoringOffersOrderBy? OrderBy { get; set; }

        public SortDirection? SortDirection { get; set; }
    }
}
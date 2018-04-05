using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class OffersQuery
    {
        public int Offset { get; set; }

        public int Count { get; set; }

        public long ExpertId { get; set; }

        public bool OnlyTimedOut { get; set; }
        
        public ScoringOfferStatus? Status { get; set; }

        public ScoringOffersOrderBy? OrderBy { get; set; }

        public SortDirection? SortDirection { get; set; }
    }
}
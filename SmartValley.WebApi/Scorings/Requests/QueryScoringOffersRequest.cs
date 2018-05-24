using SmartValley.Domain;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class QueryScoringOffersRequest
    {
        public QueryScoringOffersRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        //[Range(1, 100)]
        public int Count { get; set; }

        public ScoringOfferStatus? Status { get; set; }

        public ScoringOffersOrderBy? OrderBy { get; set; }

        public SortDirection? SortDirection { get; set; }

        public long? ScoringId { get; set; }

        public long? ExpertId { get; set; }
    }
}
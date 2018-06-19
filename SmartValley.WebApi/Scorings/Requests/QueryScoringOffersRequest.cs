using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class QueryScoringOffersRequest
    {
        public QueryScoringOffersRequest()
        {
            Count = 100;
            Statuses = new List<ScoringOfferStatus>();
        }

        [Range(0, int.MaxValue)]
        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }

        public IReadOnlyCollection<ScoringOfferStatus> Statuses { get; set; }

        public ScoringOffersOrderBy? OrderBy { get; set; }

        public SortDirection? SortDirection { get; set; }

        public long? ProjectId { get; set; }

        public long? ScoringId { get; set; }

        public long? ExpertId { get; set; }
    }
}
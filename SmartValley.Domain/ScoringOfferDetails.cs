using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringOfferDetails
    {
        public ScoringOfferStatus ScoringOfferStatus { get; set; }

        public DateTimeOffset ScoringOfferTimeStamp { get; set; }

        public string ScoringContractAddress { get; set; }

        public long ScoringId { get; set; }

        public long ExpertId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string ProjectArea { get; set; }

        public string Description { get; set; }

        public AreaType AreaType { get; set; }

        public Guid ProjectExternalId { get; set; }
    }
}
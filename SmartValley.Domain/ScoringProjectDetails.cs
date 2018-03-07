using System;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringProjectDetails
    {
        public long ProjectId { get; set; }

        public long ScoringId { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset OffersEndDate { get; set; }
    }
}

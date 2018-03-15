using System;
using SmartValley.Domain.Core;

namespace SmartValley.Domain
{
    public class ScoringProjectDetails
    {
        public long ProjectId { get; }

        public long ScoringId { get; }

        public Address Address { get; }

        public string Name { get; }

        public DateTimeOffset CreationDate { get; }

        public DateTimeOffset OffersEndDate { get; }

        public ScoringProjectDetails(long projectId, long scoringId, Address address, string name, DateTimeOffset creationDate, DateTimeOffset offersEndDate)
        {
            ProjectId = projectId;
            ScoringId = scoringId;
            Address = address;
            Name = name;
            CreationDate = creationDate;
            OffersEndDate = offersEndDate;
        }
    }
}
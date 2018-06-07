using System;
using SmartValley.Domain.Core;

namespace SmartValley.Domain
{
    public class ScoringProjectDetails
    {
        public ScoringProjectDetails(
            long projectId,
            Guid projectExternalId,
            long scoringId,
            Address contractAddress,
            string name,
            DateTimeOffset creationDate,
            DateTimeOffset acceptingDeadline)
        {
            ProjectId = projectId;
            ProjectExternalId = projectExternalId;
            ScoringId = scoringId;
            ContractAddress = contractAddress;
            Name = name;
            CreationDate = creationDate;
            AcceptingDeadline = acceptingDeadline;
        }

        public long ProjectId { get; }

        public Guid ProjectExternalId { get; }

        public long ScoringId { get; }

        public Address ContractAddress { get; }

        public string Name { get; }

        public DateTimeOffset CreationDate { get; }

        public DateTimeOffset AcceptingDeadline { get; }
    }
}
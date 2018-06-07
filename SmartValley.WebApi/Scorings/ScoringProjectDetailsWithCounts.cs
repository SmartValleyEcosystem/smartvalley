using System;
using System.Collections.Generic;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.WebApi.Projects;

namespace SmartValley.WebApi.Scorings
{
    public class ScoringProjectDetailsWithCounts : ScoringProjectDetails
    {
        public ScoringProjectDetailsWithCounts(
            ScoringProjectStatus status,
            IReadOnlyCollection<AreaExpertsCounters> areaCounts,
            long projectId,
            Guid projectExternalId,
            long scoringId,
            Address contractAddress,
            string name,
            DateTimeOffset creationDate,
            DateTimeOffset acceptingDeadline)
            : base(projectId, projectExternalId, scoringId, contractAddress, name, creationDate, acceptingDeadline)
        {
            Status = status;
            AreaCounts = areaCounts;
        }

        public ScoringProjectStatus Status { get; }

        public IReadOnlyCollection<AreaExpertsCounters> AreaCounts { get; }
    }
}
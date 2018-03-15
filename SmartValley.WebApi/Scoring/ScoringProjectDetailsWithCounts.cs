using System;
using System.Collections.Generic;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.WebApi.Projects;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringProjectDetailsWithCounts : ScoringProjectDetails
    {
        public ScoringProjectStatus Status { get; set; }

        public IEnumerable<AreaCount> AreaCounts { get; set; }

        public ScoringProjectDetailsWithCounts(
            ScoringProjectStatus status, 
            IEnumerable<AreaCount> areaCounts, 
            long projectId, 
            long scoringId, 
            Address address, 
            string name, 
            DateTimeOffset creationDate, 
            DateTimeOffset offersEndDate)
            : base(projectId, scoringId, address, name, creationDate, offersEndDate)
        {
            Status = status;
            AreaCounts = areaCounts;
        }
    }
}

using System.Collections.Generic;
using SmartValley.Domain;
using SmartValley.WebApi.Projects;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringProjectDetailsWithCounts : ScoringProjectDetails
    {
        public ScoringProjectStatus Status { get; set; }

        public IEnumerable<AreaCount> AreaCounts { get; set; }
    }
}

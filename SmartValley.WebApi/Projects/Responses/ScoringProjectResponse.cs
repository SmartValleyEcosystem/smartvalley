using System;
using System.Collections.Generic;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ScoringProjectResponse
    {
        public string ProjectId { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ScoringProjectStatus Status { get; set; }

        public IEnumerable<AreaExpertResponse> AreasExperts { get; set; }
    }
}

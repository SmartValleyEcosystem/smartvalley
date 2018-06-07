using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.WebApi.Scorings;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ScoringProjectResponse
    {
        public long ProjectId { get; set; }
        
        public string ProjectExternalId { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ScoringProjectStatus Status { get; set; }

        public IEnumerable<AreaExpertResponse> AreasExperts { get; set; }

        public static ScoringProjectResponse Create(ScoringProjectDetailsWithCounts details)
        {
            return new ScoringProjectResponse
                   {
                       Address = details.ContractAddress,
                       Name = details.Name,
                       ProjectId = details.ProjectId,
                       ProjectExternalId = details.ProjectExternalId.ToString(),
                       StartDate = details.CreationDate.Date,
                       EndDate = details.AcceptingDeadline.Date,
                       Status = details.Status,
                       AreasExperts = details.AreaCounts.Select(AreaExpertResponse.Create).ToArray()
                   };
        }
    }
}
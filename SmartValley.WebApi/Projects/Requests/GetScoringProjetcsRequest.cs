using System.Collections.Generic;

namespace SmartValley.WebApi.Projects.Requests
{
    public class GetScoringProjectsRequest
    {
        public IEnumerable<StatusRequest> Statuses { get; set; }
    }
}

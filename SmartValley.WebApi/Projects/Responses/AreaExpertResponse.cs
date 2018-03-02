using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class AreaExpertResponse
    {
        public AreaType AreaType { get; set; }

        public int AcceptedCount { get; set; }

        public int RequiredCount { get; set; }
    }
}

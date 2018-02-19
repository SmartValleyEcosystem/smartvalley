using System.Collections.Generic;

namespace SmartValley.WebApi.Experts.Responses
{
    public class ExpertResponse
    {
        public string Address { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public IReadOnlyCollection<AreaResponse> Areas { get; set; } 
    }
}

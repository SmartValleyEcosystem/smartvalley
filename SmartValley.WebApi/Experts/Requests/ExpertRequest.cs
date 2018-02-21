using System.Collections.Generic;

namespace SmartValley.WebApi.Experts.Requests
{
    public class ExpertRequest
    {
        public string TransactionHash { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }
    }
}

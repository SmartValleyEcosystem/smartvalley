using System.Collections.Generic;

namespace SmartValley.WebApi.Experts.Requests
{
    public class AcceptApplicationRequest
    {
        public long Id { get; set; }
        
        public string TransactionHash { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }
    }
}
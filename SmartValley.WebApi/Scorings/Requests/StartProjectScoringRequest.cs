using System;
using System.Collections.Generic;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class StartProjectScoringRequest
    {
        public Guid ProjectExternalId { get; set; }

        public IReadOnlyCollection<AreaRequest> Areas { get; set; }

        public string TransactionHash { get; set; }
    }
}
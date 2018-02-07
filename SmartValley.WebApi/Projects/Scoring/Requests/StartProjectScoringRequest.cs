using System;

namespace SmartValley.WebApi.Scoring.Requests
{
    public class StartProjectScoringRequest
    {
        public Guid ProjectExternalId { get; set; }

        public string TransactionHash { get; set; }
    }
}
using System.Collections.Generic;

namespace SmartValley.WebApi.Estimates
{
    public class SubmitEstimatesRequest
    {
        public long ProjectId { get; set; }

        public string ExpertAddress { get; set; }
        
        public Category Category { get; set; }

        public IReadOnlyCollection<EstimateRequest> Estimates { get; set; }
    }
}
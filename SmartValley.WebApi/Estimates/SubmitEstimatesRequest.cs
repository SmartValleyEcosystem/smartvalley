using System.Collections.Generic;

namespace SmartValley.WebApi.Estimates
{
    public class SubmitEstimatesRequest
    {
        public string TransactionHash { get; set; }

        public long ProjectId { get; set; }

        public string ExpertAddress { get; set; }

        public IReadOnlyCollection<EstimateCommentRequest> EstimateComments { get; set; }
    }
}
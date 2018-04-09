using System.Collections.Generic;
using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Estimates.Requests
{
    public class SubmitEstimatesRequest
    {
        public string TransactionHash { get; set; }

        public long ProjectId { get; set; }

        public AreaType AreaType { get; set; }

        public IReadOnlyCollection<EstimateCommentRequest> EstimateComments { get; set; }

        public string Conclusion { get; set; }
    }
}
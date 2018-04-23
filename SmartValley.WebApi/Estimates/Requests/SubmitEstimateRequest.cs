using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Estimates.Requests
{
    public class SubmitEstimateRequest
    {
        public string TransactionHash { get; set; }

        public long ProjectId { get; set; }

        public AreaType AreaType { get; set; }
    }
}
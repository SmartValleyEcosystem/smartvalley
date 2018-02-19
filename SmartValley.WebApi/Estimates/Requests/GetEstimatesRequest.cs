using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Estimates.Requests
{
    public class GetEstimatesRequest
    {
        public long ProjectId { get; set; }

        public AreaType ExpertiseArea { get; set; }
    }
}
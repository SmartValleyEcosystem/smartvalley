namespace SmartValley.WebApi.Estimates.Requests
{
    public class GetEstimatesRequest
    {
        public long ProjectId { get; set; }

        public ExpertiseArea ExpertiseArea { get; set; }
    }
}
namespace SmartValley.WebApi.Estimates
{
    public class GetEstimatesRequest
    {
        public long ProjectId { get; set; }

        public ExpertiseArea ExpertiseArea { get; set; }
    }
}
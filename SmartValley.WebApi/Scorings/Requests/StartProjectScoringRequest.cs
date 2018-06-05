namespace SmartValley.WebApi.Scorings.Requests
{
    public class StartProjectScoringRequest
    {
        public long ProjectId { get; set; }

        public string TransactionHash { get; set; }
    }
}
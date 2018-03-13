namespace SmartValley.WebApi.Experts.Requests
{
    public class SetAvailabilityRequest
    {
        public string TransactionHash { get; set; }

        public bool Value { get; set; }
    }
}
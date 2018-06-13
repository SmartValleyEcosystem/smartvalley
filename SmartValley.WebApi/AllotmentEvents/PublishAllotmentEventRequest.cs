namespace SmartValley.WebApi.AllotmentEvents
{
    public class PublishAllotmentEventRequest
    {
        public long AllotmentEventId { get; set; }
        public string TransactionHash { get; set; }
    }
}
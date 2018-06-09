namespace SmartValley.Messages.Commands
{
    public class PublishAllotmentEventCommand
    {
        public long AllotmentEventId { get; set; }
        public string TransactionHash { get; set; }
    }
}

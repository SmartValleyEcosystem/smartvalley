namespace SmartValley.Messages.Commands
{
    public class UpdateAllotmentEvent
    {
        public string TransactionHash { get; set; }

        public long AllotmentEventId { get; set; }

        public long UserId { get; set; }
    }
}
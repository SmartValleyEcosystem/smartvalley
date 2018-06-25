namespace SmartValley.Messages.Commands
{
    public class DeleteAllotmentEvent
    {
        public string TransactionHash { get; set; }

        public long AllotmentEventId { get; set; }

        public long UserId { get; set; }

        public DeleteAllotmentEvent(long userId, long allotmentEventId, string transactionHash)
        {
            UserId = userId;
            AllotmentEventId = allotmentEventId;
            TransactionHash = transactionHash;
        }
    }
}
namespace SmartValley.Messages.Commands
{
    public class StartAllotmentEvent
    {
        public StartAllotmentEvent(long allotmentEventId, long userId, string transactionHash)
        {
            TransactionHash = transactionHash;
            UserId = userId;
            AllotmentEventId = allotmentEventId;
        }

        public long UserId { get; }

        public long AllotmentEventId { get; }

        public string TransactionHash { get; }
    }
}

namespace SmartValley.Messages.Commands
{
    public class PublishAllotmentEvent
    {
        public PublishAllotmentEvent(long allotmentEventId, long userId, string transactionHash)
        {
            AllotmentEventId = allotmentEventId;
            UserId = userId;
            TransactionHash = transactionHash;
        }

        public long AllotmentEventId { get; }
        public long UserId { get; }
        public string TransactionHash { get; }
    }
}

namespace SmartValley.Messages.Commands
{
    public class ReceiveTokens
    {
        public ReceiveTokens(long userId, long allotmentEventId, string transactionHash)
        {
            UserId = userId;
            AllotmentEventId = allotmentEventId;
            TransactionHash = transactionHash;
        }

        public long UserId { get; }

        public long AllotmentEventId { get; }

        public string TransactionHash { get; }
    }
}
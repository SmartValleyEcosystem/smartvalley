namespace SmartValley.Messages.Commands
{
    public class PlaceAllotmentEventBid
    {
        public PlaceAllotmentEventBid(long userId, long allotmentEventId, string transactionHash)
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

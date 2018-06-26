namespace SmartValley.Messages.Commands
{
    public class UpdateAllotmentEvent
    {
        public UpdateAllotmentEvent(
            long allotmentEventId,
            string transactionHash,
            AllotmentEventOperation operation,
            long userId)
        {
            AllotmentEventId = allotmentEventId;
            TransactionHash = transactionHash;
            Operation = operation;
            UserId = userId;
        }

        public string TransactionHash { get; set; }

        public long AllotmentEventId { get; set; }

        public long UserId { get; set; }

        public AllotmentEventOperation Operation { get; set; }
    }
}
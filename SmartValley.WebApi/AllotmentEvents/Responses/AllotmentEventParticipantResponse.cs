namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class AllotmentEventParticipantResponse
    {
        public AllotmentEventParticipantResponse(long bid, decimal share, long userId, bool isCollected)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
            IsCollected = isCollected;
        }

        public long Bid { get; }

        public decimal Share { get; }

        public long UserId { get; }

        public bool IsCollected { get; }
    }
}
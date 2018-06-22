namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class AllotmentEventParticipantResponse
    {
        public AllotmentEventParticipantResponse(long bid, decimal share, long userId)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
        }

        public long Bid { get; }

        public decimal Share { get; }

        public long UserId { get; }
    }
}
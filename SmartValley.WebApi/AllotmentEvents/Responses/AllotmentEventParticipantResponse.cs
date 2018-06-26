namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class AllotmentEventParticipantResponse
    {
        public AllotmentEventParticipantResponse(string bid, string share, long userId, bool isCollected)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
            IsCollected = isCollected;
        }

        public string Bid { get; }

        public string Share { get; }

        public long UserId { get; }

        public bool IsCollected { get; }
    }
}
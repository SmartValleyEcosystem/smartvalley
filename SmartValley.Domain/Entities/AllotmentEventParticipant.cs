using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class AllotmentEventParticipant : Entity
    {
        private AllotmentEventParticipant()
        {
        }

        public AllotmentEventParticipant(string bid, string share, long userId, bool isCollected)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
            IsCollected = isCollected;
        }
        
        public string Bid { get; private set; }

        public string Share { get; private set; }

        public long UserId { get; private set; }

        public bool IsCollected { get; private set; }

        public long AllotmentEventId { get; private set; }
    }
}
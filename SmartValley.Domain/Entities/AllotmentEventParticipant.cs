using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class AllotmentEventParticipant : Entity
    {
        // ReSharper disable once UnusedMember.Local
        private AllotmentEventParticipant()
        {
            
        }

        public AllotmentEventParticipant(long bid, long share, long userId, bool isCollected)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
            IsCollected = isCollected;
        }

        public long Bid { get; private set; }

        public long Share { get; private set; }

        public long UserId { get; private set; }

        public bool IsCollected { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public long AllotmentEventId { get; private set; }
    }
}
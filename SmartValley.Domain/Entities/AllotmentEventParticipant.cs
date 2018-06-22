using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class AllotmentEventParticipant : Entity
    {
        // ReSharper disable once UnusedMember.Local
        private AllotmentEventParticipant()
        {
            
        }

        public AllotmentEventParticipant(long bid, long share, long userId)
        {
            Bid = bid;
            Share = share;
            UserId = userId;
        }

        public long Bid { get; private set; }

        public long Share { get; private set; }

        public long UserId { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public long AllotmentEventId { get; private set; }
    }
}
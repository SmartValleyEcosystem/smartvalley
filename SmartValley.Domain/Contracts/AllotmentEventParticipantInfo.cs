using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public class AllotmentEventParticipantInfo
    {
        public long Bid { get; set; }

        public long Share { get; set; }

        public Address Address { get; set; }

        public bool IsCollected { get; set; }
    }
}
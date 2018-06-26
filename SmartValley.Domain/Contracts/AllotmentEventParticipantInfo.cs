using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public class AllotmentEventParticipantInfo
    {
        public string Bid { get; set; }

        public string Share { get; set; }

        public Address Address { get; set; }

        public bool IsCollected { get; set; }
    }
}
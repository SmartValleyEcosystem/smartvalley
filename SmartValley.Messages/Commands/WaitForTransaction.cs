using System;

namespace SmartValley.Messages.Commands
{
    public class WaitForTransaction
    {
        public string TransactionHash { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
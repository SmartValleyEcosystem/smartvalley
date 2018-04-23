using System;
using NServiceBus;

namespace SmartValley.Application.Sagas.TransactionHandling
{
    public class TransactionHandlingSagaData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public string TransactionHash { get; set; }
    }
}
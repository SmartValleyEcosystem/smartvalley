using System;
using NServiceBus;

namespace SmartValley.Application.Sagas
{
    public class TransactionHandlingSagaData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
using System;
using NServiceBus;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventUpdateSagaData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public string TransactionHash { get; set; }

        public long AllotmentEventId { get; set; }
    }
}
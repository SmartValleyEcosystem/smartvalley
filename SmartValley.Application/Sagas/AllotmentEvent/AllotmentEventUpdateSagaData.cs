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

        public string Name { get; set; }

        public string TokenContractAddress { get; set; }

        public int TokenDecimals { get; set; }

        public string TokenTicker { get; set; }

        public DateTimeOffset? FinishDate { get; set; }
    }
}
using System;
using NServiceBus;

namespace SmartValley.Application.Sagas.Expert
{
    public class ExpertAreasUpdatingSagaData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public long ExpertId { get; set; }

        public string TransactionHash { get; set; }
    }
}
using System;
using System.Collections.Generic;
using NServiceBus;

namespace SmartValley.Application.Sagas.Scoring
{
    public class ScoringStartSagaData : IContainSagaData
    {
        public Guid Id { get; set; }

        public string Originator { get; set; }

        public string OriginalMessageId { get; set; }

        public long ProjectId { get; set; }

        public string TransactionHash { get; set; }
    }
}
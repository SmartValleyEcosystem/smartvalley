using System;
using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class AllotmentEvent : Entity
    {
        public string Name { get; set; }

        public AllotmentEventStatus Status { get; set; }

        public string TokenContractAddress { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset FinishDate { get; set; }

        public long TotalTokens { get; set; }

        public string TokenTicker { get; set; }

        public long ProjectId { get; set; }
    }
}
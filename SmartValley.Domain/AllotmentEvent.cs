using System;
using SmartValley.Domain.Core;

namespace SmartValley.Domain
{
    public class AllotmentEvent : Entity
    {
        public string Name { get; set; }

        public AllotmentEventStatus Status { get; set; }

        public string TokenContractAddress { get; set; }

        public string EventContractAddress { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? FinishDate { get; set; }

        public int TokenDecimals { get; set; }

        public string TokenTicker { get; set; }

        public bool IsUpdating { get; set; }

        public long ProjectId { get; set; }
    }
}
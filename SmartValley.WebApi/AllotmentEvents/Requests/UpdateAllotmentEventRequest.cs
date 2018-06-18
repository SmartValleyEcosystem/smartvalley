using System;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class UpdateAllotmentEventRequest
    {
        public string TransactionHash { get; set; }

        public long AllotmentEventId { get; set; }

        public string Name { get; set; }

        public string TokenContractAddress { get; set; }

        public int TokenDecimals { get; set; }

        public string TokenTicker { get; set; }

        public DateTimeOffset? FinishDate { get; set; }
    }
}
using System;
using SmartValley.Domain;

namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class AllotmentEventResponse
    {
        private AllotmentEventResponse()
        {
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public AllotmentEventStatus Status { get; set; }

        public string TokenContractAddress { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset FinishDate { get; set; }

        public long TotalTokens { get; set; }

        public string TokenTicker { get; set; }

        public static AllotmentEventResponse Create(AllotmentEvent allotmentEvent)
        {
            return new AllotmentEventResponse
                   {
                       Id = allotmentEvent.Id,
                       Name = allotmentEvent.Name,
                       Status = allotmentEvent.Status,
                       TokenContractAddress = allotmentEvent.TokenContractAddress,
                       StartDate = allotmentEvent.StartDate,
                       FinishDate = allotmentEvent.FinishDate,
                       TotalTokens = allotmentEvent.TotalTokens,
                       TokenTicker = allotmentEvent.TokenTicker
                   };
        }
    }
}
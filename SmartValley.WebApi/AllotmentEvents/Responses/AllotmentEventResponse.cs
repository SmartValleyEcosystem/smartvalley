using System;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class AllotmentEventResponse
    {
        private AllotmentEventResponse()
        {
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public long ProjectId { get; set; }

        public AllotmentEventStatus Status { get; set; }

        public string TokenContractAddress { get; set; }

        public string EventContractAddress { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? FinishDate { get; set; }

        public int TokenDecimals { get; set; }

        public string TokenTicker { get; set; }

        public string TotalTokens { get; set; }

        public AllotmentEventParticipantResponse[] Participants { get; set; }
        
        public static AllotmentEventResponse Create(AllotmentEvent allotmentEvent, DateTimeOffset now)
        {
            return new AllotmentEventResponse
                   {
                       Id = allotmentEvent.Id,
                       Name = allotmentEvent.Name,
                       ProjectId = allotmentEvent.ProjectId,
                       Status = allotmentEvent.GetActualStatus(now),
                       TokenContractAddress = allotmentEvent.TokenContractAddress,
                       EventContractAddress = allotmentEvent.EventContractAddress,
                       TotalTokens = string.IsNullOrWhiteSpace(allotmentEvent.TotalTokensToDistribute) ? "0" : allotmentEvent.TotalTokensToDistribute,
                       StartDate = allotmentEvent.StartDate,
                       FinishDate = allotmentEvent.FinishDate,
                       TokenDecimals = allotmentEvent.TokenDecimals,
                       TokenTicker = allotmentEvent.TokenTicker,
                       Participants = allotmentEvent.Participants.Select(x => new AllotmentEventParticipantResponse(x.Bid, x.Share, x.UserId, x.IsCollected)).ToArray()
                   };
        }
    }
}
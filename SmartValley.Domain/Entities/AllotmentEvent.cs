using System;
using System.Collections.Generic;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class AllotmentEvent : Entity
    {
        public AllotmentEvent()
        {
            Participants = new List<AllotmentEventParticipant>();
        }

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

        public ICollection<AllotmentEventParticipant> Participants { get; set; }

        public AllotmentEventStatus GetActualStatus(DateTimeOffset now)
        {
            if (Status == AllotmentEventStatus.InProgress && now >= FinishDate)
            {
                return AllotmentEventStatus.Finished;
            }

            return Status;
        }

        public void SetParticipants(IReadOnlyCollection<AllotmentEventParticipant> participants)
        {
            Participants.Clear();

            foreach (var participant in participants)
                Participants.Add(participant);
        }
    }
}
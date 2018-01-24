using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Votings.Responses
{
    public class SprintResponse
    {
        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public static SprintResponse Create(Voting sprint)
        {
            return new SprintResponse
            {
                Address = sprint.VotingAddress,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate
            };
        }
    }
}

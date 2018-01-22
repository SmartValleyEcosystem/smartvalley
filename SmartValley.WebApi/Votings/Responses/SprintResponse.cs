using System;
using SmartValley.Domain;

namespace SmartValley.WebApi.Votings.Responses
{
    public class SprintResponse
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long MaximumScore { get; set; }

        public SprintResponse(VotingSprintDetails sprintDetails)
        {
            StartDate = sprintDetails.StartDate;
            EndDate = sprintDetails.EndDate;
            MaximumScore = sprintDetails.MaximumScore;
        }

        public SprintResponse(DateTime startDate, DateTime endDate, long maximumScore)
        {
            StartDate = startDate;
            EndDate = endDate;
            MaximumScore = maximumScore;
        }
    }
}

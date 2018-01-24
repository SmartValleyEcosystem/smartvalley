using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class VotingSprintDetails
    {
        public VotingSprintDetails(
            string address,
            DateTime startDate, 
            DateTime endDate, 
            long acceptanceThreshold, 
            long maximumScore, 
            IReadOnlyCollection<Guid> projectsExternalIds)
        {
            Address = address;
            StartDate = startDate;
            EndDate = endDate;
            AcceptanceThreshold = acceptanceThreshold;
            MaximumScore = maximumScore;
            ProjectsExternalIds = projectsExternalIds;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public long AcceptanceThreshold { get; }

        public long MaximumScore { get; }

        public IReadOnlyCollection<Guid> ProjectsExternalIds { get; }

        public string Address { get; }
    }
}

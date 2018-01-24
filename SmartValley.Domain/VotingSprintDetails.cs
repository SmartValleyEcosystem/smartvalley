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
            int acceptanceThreshold, 
            double maximumScore, 
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

        public int AcceptanceThreshold { get; }

        public double MaximumScore { get; }

        public IReadOnlyCollection<Guid> ProjectsExternalIds { get; }

        public string Address { get; }
    }
}

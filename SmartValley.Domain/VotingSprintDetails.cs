using System;
using System.Collections.Generic;
using SmartValley.Domain.Core;

namespace SmartValley.Domain
{
    public class VotingSprintDetails
    {
        public VotingSprintDetails(
            Address address,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int acceptanceThreshold,
            double maximumScore,
            IReadOnlyCollection<Guid> projectsExternalIds,
            int number)
        {
            Address = address;
            StartDate = startDate;
            EndDate = endDate;
            AcceptanceThreshold = acceptanceThreshold;
            MaximumScore = maximumScore;
            ProjectsExternalIds = projectsExternalIds;
            Number = number;
        }

        public DateTimeOffset StartDate { get; }

        public DateTimeOffset EndDate { get; }

        public int AcceptanceThreshold { get; }

        public double MaximumScore { get; }

        public IReadOnlyCollection<Guid> ProjectsExternalIds { get; }

        public Address Address { get; }

        public int Number { get; }
    }
}

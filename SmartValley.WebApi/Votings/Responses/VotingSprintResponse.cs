using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Votings.Responses
{
    public class VotingSprintResponse
    {
        public string Address { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public double VoteBalance { get; set; }

        public double MaximumScore { get; set; }

        public int Number { get; set; }

        public int AcceptanceThreshold { get; set; }

        public IReadOnlyCollection<ProjectVoteResponse> Projects { get; set; }

        public static VotingSprintResponse Create(
            VotingSprintDetails votingSprint,
            IReadOnlyCollection<ProjectDetails> projectDetails,
            InvestorVotesDetails details,
            DateTimeOffset now)
        {
            return new VotingSprintResponse
                   {
                       Address = votingSprint.Address,
                       StartDate = votingSprint.StartDate,
                       EndDate = votingSprint.EndDate,
                       Number = votingSprint.Number,
                       Projects = SelectProjects(projectDetails, details, votingSprint.EndDate > now),
                       VoteBalance = details?.InvestorVoteBalance ?? 0,
                       MaximumScore = votingSprint.MaximumScore,
                       AcceptanceThreshold = votingSprint.AcceptanceThreshold
                   };
        }

        private static IReadOnlyCollection<ProjectVoteResponse> SelectProjects(
            IReadOnlyCollection<ProjectDetails> projectDetails,
            InvestorVotesDetails details,
            bool isVotingInProgress)
        {
            return projectDetails.Select(p => ProjectVoteResponse.Create(
                                             p,
                                             isVotingInProgress,
                                             details?.InvestorProjectVotes.FirstOrDefault(i => i.ProjectExternalId == p.Project.ExternalId)))
                                 .ToArray();
        }
    }
}
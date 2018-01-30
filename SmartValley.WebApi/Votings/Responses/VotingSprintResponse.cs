using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

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
            IReadOnlyCollection<Project> projects,
            InvestorVotesDetails details,
            DateTimeOffset now)
        {
            var votingStatus = votingSprint.EndDate > now ? VotingStatus.InProgress : VotingStatus.None;
            return new VotingSprintResponse
                   {
                       Address = votingSprint.Address,
                       StartDate = votingSprint.StartDate,
                       EndDate = votingSprint.EndDate,
                       Number = votingSprint.Number,
                       Projects = SelectProjects(projects, details, votingStatus),
                       VoteBalance = details?.TokenAmount ?? 0,
                       MaximumScore = votingSprint.MaximumScore,
                       AcceptanceThreshold = votingSprint.AcceptanceThreshold
                   };
        }

        private static IReadOnlyCollection<ProjectVoteResponse> SelectProjects(
            IReadOnlyCollection<Project> projects,
            InvestorVotesDetails details,
            VotingStatus votingStatus)
        {
            return projects.Select(project => ProjectVoteResponse.Create(
                                       project,
                                       votingStatus,
                                       details?.InvestorProjectVotes.FirstOrDefault(i => i.ProjectExternalId == project.ExternalId)))
                           .ToArray();
        }
    }
}
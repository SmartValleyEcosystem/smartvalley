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

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double VoteBalance { get; set; }

        public double MaximumScore { get; set; }

        public int Number { get; set; }

        public int AcceptanceThreshold { get; set; }

        public IReadOnlyCollection<ProjectVoteResponse> Projects { get; set; }

        public static VotingSprintResponse Create(VotingSprintDetails votingSprint, IReadOnlyCollection<Project> projects, InvestorVotesDetails details, DateTime now)
        {
            return new VotingSprintResponse
                   {
                       Address = votingSprint.Address,
                       StartDate = votingSprint.StartDate,
                       EndDate = votingSprint.EndDate,
                       Number = votingSprint.Number,
                       Projects = SelectProjects(projects, details, votingSprint.EndDate, now),
                       VoteBalance = details?.TokenAmount ?? 0,
                       MaximumScore = votingSprint.MaximumScore,
                       AcceptanceThreshold = votingSprint.AcceptanceThreshold
                   };
        }

        private static IReadOnlyCollection<ProjectVoteResponse> SelectProjects(IReadOnlyCollection<Project> projects, InvestorVotesDetails details, DateTime sprintEndDate, DateTime now)
        {
            return projects.Select(project =>
                                       ProjectVoteResponse.Create(project,
                                                                  sprintEndDate > now ? VotingStatus.InProgress : VotingStatus.None,
                                                                  details?.InvestorProjectVotes.FirstOrDefault(i => i.ProjectExternalId == project.ExternalId)))
                           .ToArray();
        }
    }
}
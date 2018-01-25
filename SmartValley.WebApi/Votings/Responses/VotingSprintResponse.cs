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

        public IReadOnlyCollection<ProjectVoteResponse> Projects { get; set; }

        public static VotingSprintResponse Create(VotingSprintDetails votingSprint, IReadOnlyCollection<Project> projects, InvestorVotes investorVotes)
        {
            return new VotingSprintResponse
                   {
                       Address = votingSprint.Address,
                       StartDate = votingSprint.StartDate,
                       EndDate = votingSprint.EndDate,
                       Projects = projects.Select(project => ProjectVoteResponse.Create(project, investorVotes)).ToArray(),
                       VoteBalance = investorVotes?.TokenAmount ?? 0,
                       MaximumScore = votingSprint.MaximumScore
            };
        }
    }
}
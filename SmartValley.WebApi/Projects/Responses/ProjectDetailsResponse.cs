using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectDetailsResponse
    {
        public string Name { get; set; }

        public string ExternalId { get; set; }

        public int CategoryId { get; set; }

        public string ScoringContractAddress { get; set; }

        public string Description { get; set; }

        public string WhitePaperLink { get; set; }

        public string Country { get; set; }

        public string VotingAddress { get; set; }

        public double? Score { get; set; }

        public string ImageUrl { get; set; }

        public ScoringStatus ScoringStatus { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public DateTimeOffset? VotingEndDate { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberResponse> TeamMembers { get; set; }

        public static ProjectDetailsResponse Create(ProjectDetails details, VotingProjectDetails votingDetails, DateTimeOffset now)
        {
            var scoringStatus = details.Scoring == null
                                    ? ScoringStatus.Pending
                                    : (details.Scoring.Score.HasValue ? ScoringStatus.Finished : ScoringStatus.InProgress);

            return new ProjectDetailsResponse
                   {
                       Name = details.Project.Name,
                       ExternalId = details.Project.ExternalId.ToString(),
                       Description = details.Project.Description,
                       Country = details.Country.Code,
                       CategoryId = (int) details.Project.Category,
                       Score = details.Scoring?.Score,
                       ScoringContractAddress = details.Scoring?.ContractAddress,
                       WhitePaperLink = details.Project?.WhitePaperLink,
                       TeamMembers = details.TeamMembers.Select(ProjectTeamMemberResponse.Create).ToList(),
                       ScoringStatus = scoringStatus,
                       VotingStatus = details.Scoring == null ? (votingDetails?.GetVotingStatus(now) ?? VotingStatus.InProgress) : VotingStatus.None,
                       VotingEndDate = votingDetails?.Voting?.EndDate,
                       VotingAddress = votingDetails?.Voting?.VotingAddress,
                       ImageUrl = details.Project.ImageUrl
                   };
        }
    }
}

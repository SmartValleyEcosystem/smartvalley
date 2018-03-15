using System;
using SmartValley.Domain;

namespace SmartValley.WebApi.Projects.Responses
{
    public class MyProjectsItemResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Author { get; set; }

        public string Country { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public double? Score { get; set; }

        public ScoringStatus ScoringStatus { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public DateTimeOffset? VotingEndDate { get; set; }

        public static MyProjectsItemResponse Create(ProjectDetails projectDetails)
        {
            return new MyProjectsItemResponse
                   {
                       Id = projectDetails.Project.Id,
                       CategoryId = (int) projectDetails.Project.CategoryId,
                       Author = projectDetails.Project.AuthorAddress,
                       Country = projectDetails.Country.Code,
                       Description = projectDetails.Project.Description,
                       Name = projectDetails.Project.Name,
                       Address = projectDetails.Scoring.ContractAddress,
                       Score = projectDetails.Scoring.Score,
                       ScoringStatus = projectDetails.Scoring.Score.HasValue ? ScoringStatus.Finished : ScoringStatus.InProgress,
                       VotingStatus = VotingStatus.None,
                       VotingEndDate = null
                   };
        }

        public static MyProjectsItemResponse Create(ProjectDetails projectDetails, VotingProjectDetails votingDetails, DateTimeOffset now)
        {
            return new MyProjectsItemResponse
                   {
                       Id = projectDetails.Project.Id,
                       CategoryId = (int) projectDetails.Project.CategoryId,
                       Author = projectDetails.Project.AuthorAddress,
                       Country = projectDetails.Country.Code,
                       Description = projectDetails.Project.Description,
                       Name = projectDetails.Project.Name,
                       Address = null,
                       Score = null,
                       ScoringStatus = ScoringStatus.Pending,
                       VotingStatus = votingDetails?.GetVotingStatus(now) ?? VotingStatus.InProgress,
                       VotingEndDate = votingDetails?.Voting?.EndDate
                   };
        }
    }
}
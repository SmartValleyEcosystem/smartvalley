using System;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class MyProjectsItemResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Author { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public string Description { get; set; }

        public double? Score { get; set; }

        public ScoringStatus ScoringStatus { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public DateTime? VotingEndDate { get; set; }

        public static MyProjectsItemResponse Create(Project project, Domain.Entities.Scoring scoring)
        {
            return new MyProjectsItemResponse
                   {
                       Id = project.Id,
                       Area = project.ProjectArea,
                       Author = project.AuthorAddress,
                       Country = project.Country,
                       Description = project.Description,
                       Name = project.Name,
                       Address = scoring.ContractAddress,
                       Score = scoring.Score,
                       ScoringStatus = scoring.Score.HasValue ? ScoringStatus.Finished : ScoringStatus.InProgress,
                       VotingStatus = VotingStatus.None,
                       VotingEndDate = null
                   };
        }

        public static MyProjectsItemResponse Create(Project project, VotingProjectDetails votingDetails, DateTime now)
        {
            return new MyProjectsItemResponse
                   {
                       Id = project.Id,
                       Area = project.ProjectArea,
                       Author = project.AuthorAddress,
                       Country = project.Country,
                       Description = project.Description,
                       Name = project.Name,
                       Address = null,
                       Score = null,
                       ScoringStatus = ScoringStatus.Pending,
                       VotingStatus = votingDetails?.GetVotingStatus(now) ?? VotingStatus.InProgress,
                       VotingEndDate = votingDetails?.Voting?.EndDate
                   };
        }
    }
}
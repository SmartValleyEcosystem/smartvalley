using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Votings.Responses
{
    public class ProjectVoteResponse
    {
        public long Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public string Description { get; set; }

        public double? MyVoteTokenAmount { get; set; }

        public double? TotalTokenAmount { get; set; }

        public bool IsVotedByMe { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public static ProjectVoteResponse Create(Project project, bool isVotingInProgress, InvestorProjectVote investorVotes)
        {
            return new ProjectVoteResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       ExternalId = project.ExternalId.ToString(),
                       Country = project.Country,
                       Area = project.ProjectArea,
                       Description = project.Description,
                       Author = project.AuthorAddress,
                       MyVoteTokenAmount = investorVotes?.InvestorTokenVote,
                       IsVotedByMe = investorVotes?.InvestorTokenVote > 0,
                       TotalTokenAmount = investorVotes?.TotalTokenVote,
                       VotingStatus = GetVotingStatus(isVotingInProgress)
                   };
        }

        private static VotingStatus GetVotingStatus(bool isVotingInProgress)
        {
            if (isVotingInProgress)
                return VotingStatus.InProgress;

            return VotingStatus.None; //TODO
        }
    }
}
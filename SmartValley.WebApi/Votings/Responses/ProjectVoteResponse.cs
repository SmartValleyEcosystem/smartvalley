using SmartValley.Domain;

namespace SmartValley.WebApi.Votings.Responses
{
    public class ProjectVoteResponse
    {
        public long Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public double? MyVoteTokenAmount { get; set; }

        public double? TotalTokenAmount { get; set; }

        public bool IsVotedByMe { get; set; }

        public VotingStatus VotingStatus { get; set; }

        public static ProjectVoteResponse Create(ProjectDetails projectDetails, bool isVotingInProgress, InvestorProjectVote investorVotes)
        {
            return new ProjectVoteResponse
                   {
                       Id = projectDetails.Project.Id,
                       Name = projectDetails.Project.Name,
                       ExternalId = projectDetails.Project.ExternalId.ToString(),
                       Country = projectDetails.Country.Code,
                       CategoryId = (int) projectDetails.Project.CategoryId,
                       Description = projectDetails.Project.Description,
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
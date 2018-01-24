using System.Linq;
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

        public bool IsVotedByMe { get; set; }

        public static ProjectVoteResponse Create(Project project, InvestorVotes investorVotes)
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
                       IsVotedByMe = investorVotes?.ProjectsExternalIds.Contains(project.ExternalId) ?? false
                   };
        }
    }
}
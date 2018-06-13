using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public Category Category { get; set; }

        public ProjectScoringResponse Scoring { get; set; }

        public string Description { get; set; }

        public bool IsApplicationSubmitted { get; set; }

        public ScoringStartTransactionStatus ScoringStartTransactionStatus { get; set; }

        public string ScoringStartTransactionHash { get; set; }

        public static ProjectResponse Create(Project project, ScoringApplication scoringApplication)
        {
            return new ProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country?.Code,
                       Category = project.Category,
                       Description = project.Description,
                       IsApplicationSubmitted = scoringApplication?.IsSubmitted ?? false,
                       ScoringStartTransactionStatus = scoringApplication?.GetTransactionStatus() ?? ScoringStartTransactionStatus.NotSubmitted,
                       ScoringStartTransactionHash = scoringApplication?.ScoringStartTransaction?.Hash,
                       Scoring = project.Scoring == null ? null : ProjectScoringResponse.Create(project.Scoring)
                   };
        }
    }
}
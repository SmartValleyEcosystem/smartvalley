using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public string Description { get; set; }

        public double? Score { get; set; }

        public static ProjectResponse Create(ProjectScoring projectScoring)
        {
            return new ProjectResponse
                   {
                       Id = projectScoring.Project.Id,
                       Name = projectScoring.Project.Name,
                       Address = projectScoring.Scoring.ContractAddress,
                       Country = projectScoring.Project.Country,
                       Area = projectScoring.Project.ProjectArea,
                       Description = projectScoring.Project.Description,
                       Score = projectScoring.Scoring.Score
                   };
        }

        public static ProjectResponse Create(Project project)
        {
            return new ProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country,
                       Area = project.ProjectArea,
                       Description = project.Description,
                       Address = null,
                       Score = null
                   };
        }
    }
}
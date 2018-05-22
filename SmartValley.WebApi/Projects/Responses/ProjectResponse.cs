using System;
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

        public static ProjectResponse Create(Project project)
        {
            return new ProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country?.Code,
                       Category = project.Category,
                       Description = project.Description,
                       Scoring = project.Scoring == null ? null : ProjectScoringResponse.Create(project.Scoring)
                   };
        }
    }
}
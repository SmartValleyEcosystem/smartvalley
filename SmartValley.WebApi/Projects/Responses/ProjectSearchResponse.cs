using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectSearchResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Category { get; set; }

        public static ProjectSearchResponse Create(Project project)
        {
            return new ProjectSearchResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country,
                       Category = project.ProjectArea
                   };
        }
    }
}
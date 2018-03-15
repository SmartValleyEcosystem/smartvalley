using SmartValley.Domain;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectSearchResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int CategoryId { get; set; }

        public static ProjectSearchResponse Create(ProjectDetails projectDetails)
        {
            return new ProjectSearchResponse
                   {
                       Id = projectDetails.Project.Id,
                       Name = projectDetails.Project.Name,
                       Country = projectDetails.Country.Code,
                       CategoryId = (int) projectDetails.Project.CategoryId
                   };
        }
    }
}
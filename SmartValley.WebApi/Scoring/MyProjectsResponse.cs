using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scoring
{
    public class MyProjectsResponse
    {
        public static MyProjectsResponse From(Project project)
        {
            return new MyProjectsResponse
            {
                       Id = project.Id,
                       ProjectName = project.Name,
                       ProjectArea = project.ProjectArea,
                       ProjectCountry = project.Country,
                       ScoringRating = project.Score,
                       ProjectDescription = project.ProblemDesc
                   };
        }

        public string ProjectDescription { get; set; }

        public double? ScoringRating { get; set; }

        public string ProjectCountry { get; set; }

        public string ProjectArea { get; set; }

        public string ProjectName { get; set; }

        public long Id { get; set; }
    }
}
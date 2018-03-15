using System;
using SmartValley.Domain;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Author { get; set; }

        public string Country { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public double? Score { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public static ProjectResponse Create(ProjectDetails projectDetails)
        {
            return new ProjectResponse
                   {
                       Id = projectDetails.Project.Id,
                       Name = projectDetails.Project.Name,
                       Country = projectDetails.Country.Code,
                       CategoryId = (int) projectDetails.Project.CategoryId,
                       Description = projectDetails.Project.Description,
                       Author = projectDetails.Project.AuthorAddress,
                       Address = projectDetails.Scoring?.ContractAddress,
                       Score = projectDetails.Scoring?.Score,
                       ScoringEndDate = projectDetails.Scoring?.ScoringEndDate
                   };
        }
    }
}
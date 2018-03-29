using System;
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

        public Category Category { get; set; }

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
                       Category = projectDetails.Project.Category,
                       Description = projectDetails.Project.Description,
                       Address = projectDetails.Scoring?.ContractAddress,
                       Score = projectDetails.Scoring?.Score,
                       ScoringEndDate = projectDetails.Scoring?.ScoringEndDate
                   };
        }
    }
}
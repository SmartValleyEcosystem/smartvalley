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

        public static ProjectResponse Create(Project project)
        {
            return new ProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country?.Code,
                       Category = project.Category,
                       Description = project.Description,
                       Address = project.Scoring?.ContractAddress,
                       Score = project.Scoring?.Score,
                       ScoringEndDate = project.Scoring?.ScoringEndDate
                   };
        }
    }
}
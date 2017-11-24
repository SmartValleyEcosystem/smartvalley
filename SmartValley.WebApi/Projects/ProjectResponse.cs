﻿using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects
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

        public static ProjectResponse From(Project project)
        {
            return new ProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Address = project.ProjectAddress,
                       Country = project.Country,
                       Area = project.ProjectArea,
                       Description = project.Description,
                       Score = project.Score
                   };
        }
    }
}
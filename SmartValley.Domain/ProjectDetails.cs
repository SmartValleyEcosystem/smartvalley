using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectDetails
    {
        public ProjectDetails(
            Project project,
            Scoring scoring,
            Country country)
        {
            Project = project;
            Scoring = scoring;
            Country = country;
        }

        public Project Project { get; }

        public Country Country { get; }

        public Scoring Scoring { get; }

        public IReadOnlyCollection<ProjectTeamMember> TeamMembers { get; set; }
    }
}
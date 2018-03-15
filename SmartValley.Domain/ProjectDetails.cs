using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectDetails
    {
        public ProjectDetails(
            Project project,
            Scoring scoring,
            Application application,
            Country country)
        {
            Project = project;
            Scoring = scoring;
            Application = application;
            Country = country;
        }

        public Project Project { get; }

        public Country Country { get; }

        public Scoring Scoring { get; }

        public Application Application { get; }

        public IReadOnlyCollection<ApplicationTeamMember> TeamMembers { get; set; }
    }
}
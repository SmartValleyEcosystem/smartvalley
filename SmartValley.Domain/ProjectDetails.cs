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
            IReadOnlyCollection<TeamMember> teamMembers)
        {
            Project = project;
            Scoring = scoring;
            Application = application;
            TeamMembers = teamMembers;
        }

        public Project Project { get; }

        public Scoring Scoring { get; }

        public Application Application { get; }

        public IReadOnlyCollection<TeamMember> TeamMembers { get; }
    }
}
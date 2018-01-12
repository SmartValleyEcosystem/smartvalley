using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectScoring
    {
        public ProjectScoring(Project project, Scoring scoring)
        {
            Project = project;
            Scoring = scoring;
        }

        public Project Project { get; }

        public Scoring Scoring { get; }
    }
}
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectStage
    {
        public long ProjectId { get; }

        public Stage Stage { get; }

        public ProjectStage(long projectId, Stage stage)
        {
            ProjectId = projectId;
            Stage = stage;
        }
    }
}
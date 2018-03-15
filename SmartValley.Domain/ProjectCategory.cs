namespace SmartValley.Domain.Entities
{
    public class ProjectCategory
    {
        public long ProjectId { get; }

        public Category Category { get; }

        public ProjectCategory(long projectId, Category category)
        {
            ProjectId = projectId;
            Category = category;
        }
    }
}
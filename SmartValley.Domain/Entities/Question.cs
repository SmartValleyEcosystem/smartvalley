using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Question : IEntityWithId
    {
        public long Id { get; set; }

        public ExpertiseArea ExpertiseArea { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }
    }
}
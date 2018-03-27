using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class ScoringCriterion : IEntityWithId
    {
        public long Id { get; set; }

        public AreaType AreaType { get; set; }

        public int Weight { get; set; }
    }
}
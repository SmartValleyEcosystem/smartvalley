using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Estimate : IEntityWithId
    {
        public long Id { get; set; }

        public string Comment { get; set; }

        public Score? Score { get; set; }

        public long ScoringCriterionId { get; set; }

        public long ExpertScoringId { get; set; }

        public ScoringCriterion ScoringCriterion { get; set; }

        public ExpertScoring ExpertScoring { get; set; }
    }
}
namespace SmartValley.Domain.Entities
{
    public class ScoringCriteriaMapping
    {
        public long Id { get; set; }

        public long ScoringCriterionId { get; set; }

        public long ScoringApplicationQuestionId { get; set; }

        public ScoringCriterion ScoringCriterion { get; set; }

        public ScoringApplicationQuestion ScoringApplicationQuestion { get; set; }
    }
}
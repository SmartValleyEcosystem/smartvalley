using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class EstimateComment : IEntityWithId
    {
        public long Id { get; set; }

        public string Comment { get; set; }

        public Score? Score { get; set; }

        public long ScoringCriterionId { get; set; }

        public long ExpertScoringConclusionId { get; set; }

        public ScoringCriterion ScoringCriterion { get; set; }

        public ExpertScoringConclusion ExpertScoringConclusion { get; set; }
    }
}
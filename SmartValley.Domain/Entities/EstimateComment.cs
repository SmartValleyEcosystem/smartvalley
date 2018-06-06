using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class EstimateComment
    {
        public long Id { get; set; }

        [Required]
        public string Comment { get; set; }

        public Score? Score { get; set; }

        public long ScoringId { get; set; }
        
        public long ExpertId { get; set; }
        
        public long ScoringCriterionId { get; set; }

        public Scoring Scoring { get; set; }

        public ScoringCriterion ScoringCriterion { get; set; }

        public Expert Expert { get; set; }
    }
}
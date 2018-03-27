using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class Estimate
    {
        public Estimate(long scoringCriterionId, Score score, string comment)
        {
            ScoringCriterionId = scoringCriterionId;
            Score = score;
            Comment = comment;
        }

        public long ScoringCriterionId { get; }

        public Score Score { get; }

        public string Comment { get; }
    }
}
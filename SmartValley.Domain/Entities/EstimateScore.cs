using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class EstimateScore
    {
        public EstimateScore(long scoringCriterionId, Score score, string expertAddress)
        {
            ScoringCriterionId = scoringCriterionId;
            Score = score;
            ExpertAddress = expertAddress;
        }

        public long ScoringCriterionId { get; }

        public Score Score { get; }

        public Address ExpertAddress { get; }
    }
}
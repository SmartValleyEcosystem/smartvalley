using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class Estimate
    {
        public Estimate(long scoringCriterionId, Score score, string comment, AreaType areaType)
        {
            ScoringCriterionId = scoringCriterionId;
            Score = score;
            Comment = comment;
            AreaType = areaType;
        }

        public long ScoringCriterionId { get; }

        public Score Score { get; }

        public string Comment { get; }

        public AreaType AreaType { get; }
    }
}
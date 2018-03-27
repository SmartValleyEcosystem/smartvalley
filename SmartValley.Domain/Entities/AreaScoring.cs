namespace SmartValley.Domain.Entities
{
    public class AreaScoring
    {
        public long ScoringId { get; set; }

        public AreaType AreaId { get; set; }

        public double? Score { get; set; }

        public int ExpertsCount { get; set; }

        public Scoring Scoring { get; set; }

        public Area Area{ get; set; }
    }
}
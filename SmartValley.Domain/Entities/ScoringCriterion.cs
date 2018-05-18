namespace SmartValley.Domain.Entities
{
    public class ScoringCriterion
    {
        public long Id { get; set; }

        public AreaType AreaType { get; set; }

        public int Weight { get; set; }

        public string GroupKey { get; set; }

        public int GroupOrder { get; set; }

        public int Order { get; set; }

        public bool HasMiddleScoreOption { get; set; }
    }
}
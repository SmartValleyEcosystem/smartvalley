using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class ExpertScoring
    {
        public long Id { get; set; }

        public long ExpertId { get; set; }

        public long ScoringId { get; set; }

        public AreaType Area { get; set; }

        public string Conclusion { get; set; }

        public Expert Expert { get; set; }

        public Scoring Scoring { get; set; }

        public ICollection<Estimate> Estimates { get; set; }
    }
}
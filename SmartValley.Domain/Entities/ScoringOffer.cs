namespace SmartValley.Domain.Entities
{
    public class ScoringOffer
    {
        public long ScoringId { get; set; }

        public long ExpertId { get; set; }

        public AreaType AreaId { get; set; }

        public ScoringOfferStatus Status { get; set; }

        public Scoring Scoring { get; set; }

        public Expert Expert { get; set; }
    }
}
namespace SmartValley.Domain.Entities
{
    public class ScoringOffer
    {
        // ReSharper disable once UnusedMember.Local
        private ScoringOffer()
        {
            
        }

        public ScoringOffer(long expertId, AreaType areaId, ScoringOfferStatus status)
        {
            ExpertId = expertId;
            AreaId = areaId;
            Status = status;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public long ScoringId { get; private set; }

        public long ExpertId { get; private set; }

        public AreaType AreaId { get; private set; }

        public ScoringOfferStatus Status { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Expert Expert { get; private set; }
    }
}
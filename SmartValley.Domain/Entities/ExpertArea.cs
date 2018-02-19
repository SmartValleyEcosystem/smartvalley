namespace SmartValley.Domain.Entities
{
    public class ExpertArea
    {
        public long ExpertId { get; set; }

        public AreaType AreaId { get; set; }

        public Expert Expert { get; set; }
    }
}

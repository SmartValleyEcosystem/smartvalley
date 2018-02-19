namespace SmartValley.Domain.Entities
{
    public class ExpertApplicationArea
    {
        public long ExpertApplicationId { get; set; }

        public AreaType AreaId { get; set; }

        public virtual ExpertApplication ExpertApplication { get; set; }
    }
}
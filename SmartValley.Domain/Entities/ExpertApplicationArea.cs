namespace SmartValley.Domain.Entities
{
    public class ExpertApplicationArea
    {
        public long ExpertApplicationId { get; set; }

        public ExpertiseAreaType ExpertiseAreaType { get; set; }

        public virtual ExpertApplication ExpertApplication { get; set; }
    }
}
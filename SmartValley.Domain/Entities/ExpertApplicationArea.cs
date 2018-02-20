namespace SmartValley.Domain.Entities
{
    public class ExpertApplicationArea
    {
        public long ExpertApplicationId { get; set; }

        public ExpertApplication ExpertApplication { get; set; }

        public AreaType AreaId { get; set; }

        public ExpertApplicationStatus Status { get; set; }
    }
}
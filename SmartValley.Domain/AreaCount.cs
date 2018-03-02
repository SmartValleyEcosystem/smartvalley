using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class AreaCount
    {
        public AreaType AreaType { get; set; }
        public int AcceptedCount { get; set; }
        public int RequeiredCount { get; set; }
    }
}

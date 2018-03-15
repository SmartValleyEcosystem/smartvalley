using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class AreaCount
    {
        public AreaCount(AreaType areaType, int acceptedCount, int requeiredCount)
        {
            AreaType = areaType;
            AcceptedCount = acceptedCount;
            RequeiredCount = requeiredCount;
        }

        public AreaType AreaType { get; }

        public int AcceptedCount { get; }

        public int RequeiredCount { get; }
    }
}
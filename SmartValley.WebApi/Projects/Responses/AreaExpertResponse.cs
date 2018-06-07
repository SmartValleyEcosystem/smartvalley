using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class AreaExpertResponse
    {
        public AreaType AreaType { get; set; }

        public int AcceptedCount { get; set; }

        public int RequiredCount { get; set; }

        public static AreaExpertResponse Create(AreaExpertsCounters area)
        {
            return new AreaExpertResponse
                   {
                       AreaType = area.AreaType,
                       AcceptedCount = area.AcceptedCount,
                       RequiredCount = area.RequeiredCount
                   };
        }
    }
}
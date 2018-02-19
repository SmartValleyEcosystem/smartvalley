using System;

namespace SmartValley.WebApi.Experts
{
    public static class ExpertiseAreaTranslator
    {
        public static Domain.Entities.AreaType ToDomain(this AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.Hr:
                    return Domain.Entities.AreaType.Hr;
                case AreaType.Analyst:
                    return Domain.Entities.AreaType.Analyst;
                case AreaType.Tech:
                    return Domain.Entities.AreaType.Tech;
                case AreaType.Lawyer:
                    return Domain.Entities.AreaType.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(areaType), areaType, null);
            }
        }

        public static AreaType FromDomain(this Domain.Entities.AreaType areaType)
        {
            switch (areaType)
            {
                case Domain.Entities.AreaType.Hr:
                    return AreaType.Hr;
                case Domain.Entities.AreaType.Analyst:
                    return AreaType.Analyst;
                case Domain.Entities.AreaType.Tech:
                    return AreaType.Tech;
                case Domain.Entities.AreaType.Lawyer:
                    return AreaType.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(areaType), areaType, null);
            }
        }
    }
}
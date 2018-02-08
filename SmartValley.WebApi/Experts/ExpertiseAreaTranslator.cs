using System;

namespace SmartValley.WebApi.Experts
{
    public static class ExpertiseAreaTranslator
    {
        public static Domain.Entities.ExpertiseAreaType ToDomain(this ExpertiseAreaType expertiseAreaType)
        {
            switch (expertiseAreaType)
            {
                case ExpertiseAreaType.Hr:
                    return Domain.Entities.ExpertiseAreaType.Hr;
                case ExpertiseAreaType.Analyst:
                    return Domain.Entities.ExpertiseAreaType.Analyst;
                case ExpertiseAreaType.Tech:
                    return Domain.Entities.ExpertiseAreaType.Tech;
                case ExpertiseAreaType.Lawyer:
                    return Domain.Entities.ExpertiseAreaType.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expertiseAreaType), expertiseAreaType, null);
            }
        }

        public static ExpertiseAreaType FromDomain(this Domain.Entities.ExpertiseAreaType expertiseAreaType)
        {
            switch (expertiseAreaType)
            {
                case Domain.Entities.ExpertiseAreaType.Hr:
                    return ExpertiseAreaType.Hr;
                case Domain.Entities.ExpertiseAreaType.Analyst:
                    return ExpertiseAreaType.Analyst;
                case Domain.Entities.ExpertiseAreaType.Tech:
                    return ExpertiseAreaType.Tech;
                case Domain.Entities.ExpertiseAreaType.Lawyer:
                    return ExpertiseAreaType.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expertiseAreaType), expertiseAreaType, null);
            }
        }
    }
}
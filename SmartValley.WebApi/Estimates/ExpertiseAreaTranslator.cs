using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public static class ExpertiseAreaTranslator
    {
        public static Domain.Entities.ExpertiseArea ToDomain(this ExpertiseArea requestExpertiseArea)
        {
            switch (requestExpertiseArea)
            {
                case ExpertiseArea.Hr:
                    return Domain.Entities.ExpertiseArea.Hr;
                case ExpertiseArea.Analyst:
                    return Domain.Entities.ExpertiseArea.Analyst;
                case ExpertiseArea.Tech:
                    return Domain.Entities.ExpertiseArea.Tech;
                case ExpertiseArea.Lawyer:
                    return Domain.Entities.ExpertiseArea.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestExpertiseArea), requestExpertiseArea, null);
            }
        }
    }
}
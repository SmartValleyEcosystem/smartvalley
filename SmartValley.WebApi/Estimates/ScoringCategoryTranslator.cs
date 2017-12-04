using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public static class ScoringCategoryTranslator
    {
        public static ExpertiseArea ToDomain(this ExpertiseAreaApi requestExpertiseAreaApi)
        {
            switch (requestExpertiseAreaApi)
            {
                case ExpertiseAreaApi.Hr:
                    return ExpertiseArea.Hr;
                case ExpertiseAreaApi.Analyst:
                    return ExpertiseArea.Analyst;
                case ExpertiseAreaApi.Tech:
                    return ExpertiseArea.Tech;
                case ExpertiseAreaApi.Lawyer:
                    return ExpertiseArea.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestExpertiseAreaApi), requestExpertiseAreaApi, null);
            }
        }
    }
}
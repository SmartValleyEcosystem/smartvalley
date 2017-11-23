using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public static class ScoringCategoryTranslator
    {
        public static ScoringCategory ToDomain(this Category requestCategory)
        {
            switch (requestCategory)
            {
                case Category.Hr:
                    return ScoringCategory.Hr;
                case Category.Analyst:
                    return ScoringCategory.Analyst;
                case Category.Tech:
                    return ScoringCategory.Tech;
                case Category.Lawyer:
                    return ScoringCategory.Lawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestCategory), requestCategory, null);
            }
        }
    }
}
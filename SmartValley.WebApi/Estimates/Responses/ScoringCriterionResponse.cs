using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts;
using AreaType = SmartValley.WebApi.Experts.AreaType;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringCriterionResponse
    {
        public long Id { get; set; }

        public AreaType AreaType { get; set; }

        public int Weight { get; set; }

        public static ScoringCriterionResponse From(ScoringCriterion scoringCriterion)
        {
            return new ScoringCriterionResponse
                   {
                       Id = scoringCriterion.Id,
                       AreaType = scoringCriterion.AreaType.FromDomain(),
                       Weight = scoringCriterion.Weight
                   };
        }
    }
}
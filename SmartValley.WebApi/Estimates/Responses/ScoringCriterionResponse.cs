using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringCriterionResponse
    {
        public long Id { get; set; }

        public int Weight { get; set; }

        public bool HasMiddleScoreOption { get; set; }

        public int Order { get; set; }

        public static ScoringCriterionResponse From(ScoringCriterion scoringCriterion)
        {
            return new ScoringCriterionResponse
                   {
                       Id = scoringCriterion.Id,
                       Weight = scoringCriterion.Weight,
                       HasMiddleScoreOption = scoringCriterion.HasMiddleScoreOption,
                       Order = scoringCriterion.Order
                   };
        }
    }
}
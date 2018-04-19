using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class EstimateResponse
    {
        public int? Score { get; set; }

        public string Comment { get; set; }

        public long ScoringCriterionId { get; set; }

        public static EstimateResponse Create(EstimateComment estimate)
        {
            return new EstimateResponse
                   {
                       Score = estimate.Score.HasValue ? (int?)estimate.Score.Value : null,
                       Comment = estimate.Comment,
                       ScoringCriterionId = estimate.ScoringCriterionId
            };
        }
    }
}
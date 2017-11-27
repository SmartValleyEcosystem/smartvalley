using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public class EstimateResponse
    {
        public int QuestionIndex { get; set; }

        public int Score { get; set; }

        public string Comment { get; set; }

        public static EstimateResponse From(Estimate estimate)
        {
            return new EstimateResponse
                   {
                       Score = estimate.Score,
                       Comment = estimate.Comment,
                       QuestionIndex = estimate.QuestionIndex
                   };
        }
    }
}
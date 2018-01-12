using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class EstimateResponse
    {
        public int Score { get; set; }

        public string Comment { get; set; }

        public static EstimateResponse Create(Estimate estimate)
        {
            return new EstimateResponse
                   {
                       Score = estimate.Score,
                       Comment = estimate.Comment
                   };
        }
    }
}
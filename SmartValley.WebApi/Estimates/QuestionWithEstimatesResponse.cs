using System.Collections.Generic;

namespace SmartValley.WebApi.Estimates
{
    public class QuestionWithEstimatesResponse
    {
        public long QuestionId { get; set; }

        public IReadOnlyCollection<EstimateResponse> Estimates { get; set; }
    }
}
using System.Collections.Generic;

namespace SmartValley.WebApi.Estimates
{
    public class GetEstimatesResponse
    {
        public double AverageScore { get; set; }

        public IReadOnlyCollection<EstimateResponse> Items { get; set; }
    }
}
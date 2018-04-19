using System.Collections.Generic;
using SmartValley.WebApi.Estimates.Responses;

namespace SmartValley.WebApi.Estimates
{
    public class ExpertEstimateResponse
    {
        public string Conclusion { get; set; }

        public IReadOnlyCollection<EstimateResponse> Estimates { get; set; }

        public static ExpertEstimateResponse Empty => new ExpertEstimateResponse
                                                      {
                                                          Estimates = new EstimateResponse[0]
                                                      };
    }
}
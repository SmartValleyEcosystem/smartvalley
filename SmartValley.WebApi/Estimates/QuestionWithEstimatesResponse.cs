using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates
{
    public class QuestionWithEstimatesResponse
    {
        public int QuestionId { get; set; }

        public IReadOnlyCollection<EstimateResponse> Estimates { get; set; }
    }
}
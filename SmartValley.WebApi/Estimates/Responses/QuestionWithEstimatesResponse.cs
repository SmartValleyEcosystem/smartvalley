using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class QuestionWithEstimatesResponse
    {
        public long QuestionId { get; set; }

        public IReadOnlyCollection<EstimateResponse> Estimates { get; set; }

        public static QuestionWithEstimatesResponse Create(long questionId, IReadOnlyCollection<Estimate> estimates)
        {
            return new QuestionWithEstimatesResponse
                   {
                       QuestionId = questionId,
                       Estimates = estimates.Select(EstimateResponse.Create).ToArray()
                   };
        }
    }
}
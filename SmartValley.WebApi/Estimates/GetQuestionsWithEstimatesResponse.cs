using System.Collections.Generic;

namespace SmartValley.WebApi.Estimates
{
    public class GetQuestionsWithEstimatesResponse
    {
        public double AverageScore { get; set; }

        public IReadOnlyCollection<QuestionWithEstimatesResponse> Questions { get; set; }
    }
}
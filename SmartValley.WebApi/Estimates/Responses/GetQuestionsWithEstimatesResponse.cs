using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class GetQuestionsWithEstimatesResponse
    {
        public double? AverageScore { get; set; }

        public IReadOnlyCollection<QuestionWithEstimatesResponse> Questions { get; set; }

        public static GetQuestionsWithEstimatesResponse Create(ScoringStatisticsInArea scoringStatisticsInArea)
        {
            return new GetQuestionsWithEstimatesResponse
                   {
                       AverageScore = scoringStatisticsInArea.AverageScore,
                       Questions = scoringStatisticsInArea
                           .Estimates
                           .GroupBy(e => e.QuestionId)
                           .Select(q => QuestionWithEstimatesResponse.Create(q.Key, q.ToArray()))
                           .ToArray()
                   };
        }
    }
}
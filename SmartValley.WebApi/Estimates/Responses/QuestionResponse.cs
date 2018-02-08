using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts;
using ExpertiseAreaType = SmartValley.WebApi.Experts.ExpertiseAreaType;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class QuestionResponse
    {
        public long Id { get; set; }

        public ExpertiseAreaType ExpertiseArea { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }
        
        public static QuestionResponse From(Question question)
        {
            return new QuestionResponse
                   {
                       Id = question.Id,
                       ExpertiseArea = question.ExpertiseAreaType.FromDomain(),
                       MaxScore = question.MaxScore,
                       MinScore = question.MinScore
                   };
        }
    }
}
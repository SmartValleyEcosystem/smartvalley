using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts;
using AreaType = SmartValley.WebApi.Experts.AreaType;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class QuestionResponse
    {
        public long Id { get; set; }

        public AreaType AreaType { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }
        
        public static QuestionResponse From(Question question)
        {
            return new QuestionResponse
                   {
                       Id = question.Id,
                       AreaType = question.AreaType.FromDomain(),
                       MaxScore = question.MaxScore,
                       MinScore = question.MinScore
                   };
        }
    }
}
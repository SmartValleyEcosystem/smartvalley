using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class EstimateScore
    {
        public EstimateScore(long questionId, int score, string expertAddress)
        {
            QuestionId = questionId;
            Score = score;
            ExpertAddress = expertAddress;
        }

        public long QuestionId { get; }

        public int Score { get; }

        public Address ExpertAddress { get; }
    }
}
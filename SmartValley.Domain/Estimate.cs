namespace SmartValley.Domain
{
    public class Estimate
    {
        public Estimate(long questionId, int score, string comment)
        {
            QuestionId = questionId;
            Score = score;
            Comment = comment;
        }

        public long QuestionId { get; }

        public int Score { get; }

        public string Comment { get; }
    }
}
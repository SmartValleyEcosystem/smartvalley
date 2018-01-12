namespace SmartValley.Domain
{
    public class Estimate
    {
        public Estimate(long projectId, string expertAddress, long questionId, int score, string comment)
        {
            ProjectId = projectId;
            ExpertAddress = expertAddress;
            QuestionId = questionId;
            Score = score;
            Comment = comment;
        }

        public long ProjectId { get; }

        public string ExpertAddress { get; }

        public long QuestionId { get; }

        public int Score { get; }

        public string Comment { get; }
    }
}
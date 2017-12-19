namespace SmartValley.Domain
{
    public class Estimate
    {
        public long ProjectId { get; set; }

        public string ExpertAddress { get; set; }

        public long QuestionId { get; set; }

        public int Score { get; set; }

        public string Comment { get; set; }
    }
}
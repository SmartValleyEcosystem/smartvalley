namespace SmartValley.Domain
{
    public class ScoringApplicationAnswer
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public string Value { get; set; }

        public Entities.ScoringApplicationQuestion Question { get; set; }
    }
}
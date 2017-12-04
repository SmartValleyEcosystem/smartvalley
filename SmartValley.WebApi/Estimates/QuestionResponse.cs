namespace SmartValley.WebApi.Estimates
{
    public class QuestionResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ExpertiseAreaApi ExpertiseArea { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }
    }
}
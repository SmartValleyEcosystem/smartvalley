namespace SmartValley.Domain.Entities
{
    public class ScoringApplicationQuestion
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public QuestionControlType Type { get; set; }

        public string ExtendedInfo { get; set; }

        public long? ParentId { get; set; }

        public string ParentTriggerValue { get; set; }

        public string GroupKey { get; set;}

        public int GroupOrder { get; set; }

        public int Order { get; set; }
    }
}
using System.Collections.Generic;

namespace SmartValley.WebApi.ScoringApplication.Responses
{
    public class ApplicationPartition
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public IEnumerable<ScoringApplicationQuestion> Questions { get; set; }
    }
}
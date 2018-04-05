using System.Collections.Generic;

namespace SmartValley.WebApi.ScoringApplications.Responses
{
    public class ApplicationPartitionResponse
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public IEnumerable<ScoringApplicationQuestionResponse> Questions { get; set; }
    }
}
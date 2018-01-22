using System;

namespace SmartValley.WebApi.Votings.Responses
{
    public class SprintResponse
    {
        public bool IsExist { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long MaximumScore { get; set; }
    }
}

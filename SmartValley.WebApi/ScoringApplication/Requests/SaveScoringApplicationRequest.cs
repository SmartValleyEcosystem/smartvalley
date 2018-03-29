using System;
using System.Collections.Generic;

namespace SmartValley.WebApi.ScoringApplication.Requests
{
    public class SaveScoringApplicationRequest
    {
        public SaveScoringApplicationRequest()
        {
            Answers = new Dictionary<int, string>();
            Advisers = new List<AdviserRequest>();
            TeamMembers = new List<TeamMemberRequest>();
        }

        public string ProjectName { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        public string ProjectDescription { get; set; }

        public string CountryCode { get; set; }

        public string Site { get; set; }

        public string WhitePaper { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public SocialNetworkRequest SocialNetworks { get; set; }

        public IDictionary<int, string> Answers { get; set; }

        public IReadOnlyCollection<TeamMemberRequest> TeamMembers { get; set; }

        public IReadOnlyCollection<AdviserRequest> Advisers { get; set; }
    }
}
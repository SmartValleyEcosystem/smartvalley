using System.Collections.Generic;

namespace SmartValley.WebApi.ScoringApplication.Requests
{
    public class SaveScoringApplicationRequest
    {
        public SaveScoringApplicationRequest()
        {
            Answers = new List<ScoringApplicationAnswerRequest>();
            Advisers = new List<AdviserRequest>();
            TeamMembers = new List<TeamMemberRequest>();
        }

        public string ProjectName { get; set; }

        public string ProjectArea { get; set; }

        public string Status { get; set; }

        public string ProjectDescription { get; set; }

        public string CountryCode { get; set; }

        public string Site { get; set; }

        public string WhitePaper { get; set; }

        public string IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public SocialNetworkRequest SocialNetworks { get; set; }

        public IReadOnlyCollection<ScoringApplicationAnswerRequest> Answers { get; set; }

        public IReadOnlyCollection<TeamMemberRequest> TeamMembers { get; set; }

        public IReadOnlyCollection<AdviserRequest> Advisers { get; set; }
    }
}
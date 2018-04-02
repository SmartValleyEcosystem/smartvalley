using System;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

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

        public Category? ProjectCategory { get; set; }

        public Stage? ProjectStage { get; set; }

        public string ProjectDescription { get; set; }

        public string CountryCode { get; set; }

        public string Site { get; set; }

        public string WhitePaper { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public SocialNetworkRequest SocialNetworks { get; set; }

        public IReadOnlyCollection<ScoringApplicationAnswerRequest> Answers { get; set; }

        public IReadOnlyCollection<TeamMemberRequest> TeamMembers { get; set; }

        public IReadOnlyCollection<AdviserRequest> Advisers { get; set; }
    }
}
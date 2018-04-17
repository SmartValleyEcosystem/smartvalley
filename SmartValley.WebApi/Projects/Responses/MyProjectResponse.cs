using System;
using System.Collections.Generic;
using SmartValley.WebApi.Projects.Requests;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects.Responses
{
    public class MyProjectResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Category { get; set; }

        public int Stage { get; set; }

        public string ImageUrl { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public string Facebook { get; set; }

        public string Linkedin { get; set; }

        public string BitcoinTalk { get; set; }

        public string Medium { get; set; }

        public string Reddit { get; set; }

        public string Telegram { get; set; }

        public string Twitter { get; set; }

        public string Github { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberResponse> TeamMembers { get; set; }
    }
}
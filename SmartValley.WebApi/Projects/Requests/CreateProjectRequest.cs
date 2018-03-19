using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public string AuthorAddress { get; set; }

        public int CategoryId { get; set; }

        public int StageId { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        [Url]
        public string Website { get; set; }

        [Url]
        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        public string ProjectId { get; set; }

        public IReadOnlyCollection<SocialMediaRequest> SocialMedias { get; set; }

        public IReadOnlyCollection<TeamMemberRequest> TeamMembers { get; set; }
    }
}
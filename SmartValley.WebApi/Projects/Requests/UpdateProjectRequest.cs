using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects.Requests
{
    public class UpdateProjectRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Url]
        [Required]
        public string Website { get; set; }

        [Url]
        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        [Url, MaxLength(500)]
        public string Facebook { get; set; }

        [Url, MaxLength(500)]
        public string Linkedin { get; set; }

        [Url, MaxLength(500)]
        public string BitcoinTalk { get; set; }

        [Url, MaxLength(500)]
        public string Medium { get; set; }

        [Url, MaxLength(500)]
        public string Reddit { get; set; }

        [Url, MaxLength(500)]
        public string Telegram { get; set; }

        [Url, MaxLength(500)]
        public string Twitter { get; set; }

        [Url, MaxLength(500)]
        public string Github { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberRequest> TeamMembers { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Projects.Requests
{
    public class CreateProjectRequest
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public int Stage { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Url]
        [MaxLength(200)]
        public string Website { get; set; }

        [Url]
        [MaxLength(200)]
        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        public string ContactEmail { get; set; }

        [Url, MaxLength(200)]
        public string Facebook { get; set; }

        [Url, MaxLength(200)]
        public string Linkedin { get; set; }

        [Url, MaxLength(200)]
        public string BitcoinTalk { get; set; }

        [Url, MaxLength(200)]
        public string Medium { get; set; }

        [Url, MaxLength(200)]
        public string Reddit { get; set; }

        [Url, MaxLength(200)]
        public string Telegram { get; set; }

        [Url, MaxLength(200)]
        public string Twitter { get; set; }

        [Url, MaxLength(200)]
        public string Github { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberRequest> TeamMembers { get; set; }
    }
}
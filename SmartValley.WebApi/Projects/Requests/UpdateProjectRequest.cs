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

        public string ExternalId { get; set; }

        public SocialNetworkRequest SocialNetworks { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberRequest> TeamMembers { get; set; }
    }
}
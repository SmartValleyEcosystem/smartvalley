using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project
    {
        public Project()
        {
            TeamMembers = new List<ProjectTeamMember>();
        }

        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public long CountryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public Stage Stage { get; set; }

        [Required]
        public long AuthorId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Website { get; set; }

        [MaxLength(200)]
        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        public string ContactEmail { get; set; }

        [Url, MaxLength(200)]
        public string ImageUrl { get; set; }

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
        
        public bool IsPrivate { get; set; }

        public User Author { get; set; }

        public Country Country { get; set; }

        public Scoring Scoring { get; set; }

        public ICollection<ProjectTeamMember> TeamMembers { get; }

        public ICollection<AllotmentEvent> AllotmentEvents { get; set; }

        public void UpdateTeamMemberPhotoLink(long memberId, string link)
        {
            var member = TeamMembers.FirstOrDefault(i => i.Id == memberId);
            if (member != null)
                member.PhotoUrl = link;
        }

        public ProjectTeamMember GetTeamMember(long memberId)
        {
            return TeamMembers.FirstOrDefault(i => i.Id == memberId);
        }

        public void UpdateMembers(IReadOnlyCollection<ProjectTeamMember> modifiedMembers)
        {
            TeamMembers.Merge(modifiedMembers);
        }
    }
}
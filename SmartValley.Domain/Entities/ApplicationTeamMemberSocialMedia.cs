using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class ApplicationTeamMemberSocialMedia
    {
        public long TeamMemberId { get; set; }

        public SocialMediaType SocialMediaId { get; set; }

        [Required, Url]
        public string Url { get; set; }

        public SocialMedia SocialMedia { get; set; }

        public ProjectTeamMember TeamMember { get; set; }
    }
}

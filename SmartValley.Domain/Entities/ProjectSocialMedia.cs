using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class ProjectSocialMedia
    {
        public long ProjectId { get; set; }

        public SocialMediaType SocialMediaId { get; set; }

        [Required, Url]
        public string Url { get; set; }

        public SocialMedia SocialMedia { get; set; }

        public Project Project { get; set; }
    }
}
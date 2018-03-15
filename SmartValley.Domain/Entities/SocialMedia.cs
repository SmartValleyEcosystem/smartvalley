using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class SocialMedia
    {
        public SocialMediaType Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<ProjectSocialMedia> ProjectSocialMedias { get; set; }
    }
}

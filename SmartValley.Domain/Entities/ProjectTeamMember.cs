using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class ProjectTeamMember : IEntityWithId
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        [Url, MaxLength(200)]
        public string PhotoUrl { get; set; }

        [Url, MaxLength(200)]
        public string Facebook { get; set; }

        [Url, MaxLength(200)]
        public string Linkedin { get; set; }

        public Project Project { get; set; }
    }
}
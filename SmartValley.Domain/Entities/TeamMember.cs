using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class TeamMember : IEntityWithId
    {
        public long Id { get; set; }

        public long ApplicationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Url]
        [MaxLength(100)]
        public string FacebookLink { get; set; }

        [Url]
        [MaxLength(100)]
        public string LinkedInLink { get; set; }

        public MemberType PersonType { get; set; }
    }
}

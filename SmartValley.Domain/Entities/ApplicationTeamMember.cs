using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class ApplicationTeamMember : IEntityWithId
    {
        public long Id { get; set; }

        public long ApplicationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        [MaxLength(50)]
        public string PhotoName { get; set; }

        public Application Application { get; set; }
    }
}
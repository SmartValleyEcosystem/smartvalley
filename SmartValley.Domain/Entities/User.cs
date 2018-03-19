using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class User : IEntityWithId
    {
        public long Id { get; set; }
        
        [Required]
        [MaxLength(42)]
        public Address Address { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public Expert Expert { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
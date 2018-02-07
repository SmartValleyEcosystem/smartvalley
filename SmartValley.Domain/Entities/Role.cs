using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Role
    {
        public RoleType Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
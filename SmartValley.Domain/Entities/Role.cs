using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Role : IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
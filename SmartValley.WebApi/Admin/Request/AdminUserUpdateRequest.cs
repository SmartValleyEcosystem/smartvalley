using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.WebApi.Admin.Request
{
    public class AdminUserUpdateRequest
    {
        [Required]
        public Address Address { get; set; }

        public bool CanCreatePrivateProjects { get; set; }
    }
}

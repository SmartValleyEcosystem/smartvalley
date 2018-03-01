using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string About { get; set; }
    }
}
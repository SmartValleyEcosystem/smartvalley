using System.ComponentModel.DataAnnotations;
using SmartValley.WebApi.Users.Requests;

namespace SmartValley.WebApi.Admin.Request
{
    public class AdminExpertUpdateRequest : UpdateUserRequest
    {
        [Required]
        public string Address { get; set; }

        public string Email { get; set; }

        public string TransactionHash { get; set; }

        public bool IsInHouse { get; set; }

        public bool IsAvailable { get; set; }
    }
}

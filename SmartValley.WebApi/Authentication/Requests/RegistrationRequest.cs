using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Authentication.Requests
{
    public class RegistrationRequest
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string SignedText { get; set; }

        [Required]
        public string Signature { get; set; }
    }
}
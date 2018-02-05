using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Authentication.Requests
{
    public class AuthenticationRequest
    {
        [Required]
        public string Address { get; set; }

        public string SignedText { get; set; }

        public string Signature { get; set; }
    }
}
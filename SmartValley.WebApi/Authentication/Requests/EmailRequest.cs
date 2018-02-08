using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Authentication.Requests
{
    public class EmailRequest
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string SignedText { get; set; }

        [Required]
        public string Signature { get; set; }
    }
}
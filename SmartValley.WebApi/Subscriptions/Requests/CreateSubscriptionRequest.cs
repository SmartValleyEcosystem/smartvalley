using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Subscriptions.Requests
{
    public class CreateSubscriptionRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Sum { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }
    }
}

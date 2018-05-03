using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Feedbacks.Requests
{
    public class CreateFeedbackRequest
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(1500)]
        public string Text { get; set; }
    }
}

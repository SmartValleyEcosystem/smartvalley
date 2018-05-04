using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Feedbacks.Requests
{
    public class CreateFeedbackRequest
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(1500)]
        public string Text { get; set; }
    }
}

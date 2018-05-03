using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Feedback
    {
        public Feedback(string firstName, string lastName, string email, string text)
        {
            firstName = FirstName;
            lastName = LastName;
            email = Email;
            text = Text;
        }

        public long Id { get; set; }

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

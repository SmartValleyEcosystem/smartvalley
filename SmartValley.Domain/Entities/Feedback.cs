using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Feedback
    {
        public Feedback(string firstName, string lastName, string email, string text)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Text = text;
        }

        public long Id { get; set; }

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

using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Feedbacks.Responses
{
    public class FeedbackResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public static FeedbackResponse Create(Feedback feedback)
        {
            return new FeedbackResponse
                   {
                       FirstName = feedback.FirstName,
                       LastName = feedback.LastName,
                       Email = feedback.Email,
                       Text = feedback.Text
                   };
        }
    }
}
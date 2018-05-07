using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Subscribers.Responses
{
    public class SubscriptionResponse
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Sum { get; set; }

        public string Phone { get; set; }

        public long ProjectId { get; set; }

        public string ProjectName { get; set; }

        public static SubscriptionResponse Create(Subscription subscriber, Project project)
        {
            return new SubscriptionResponse
                   {
                       ProjectId = subscriber.ProjectId,
                       ProjectName = project.Name,
                       Name = subscriber.Name,
                       Sum = subscriber.Sum,
                       Email = subscriber.Email,
                       Phone = subscriber.Phone
                   };
        }
    }
}
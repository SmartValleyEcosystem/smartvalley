using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Subscription
    {
        public Subscription(long projectId, string name, string phone, string email, string sum)
        {
            ProjectId = projectId;
            Name = name;
            Phone = phone;
            Email = email;
            Sum = sum;
        }

        public long Id { get; set; }
        
        public long ProjectId { get; set; }
        
        public string Name { get; set; }
        
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        public string Sum { get; set; }

        public Project Project { get; set; }
    }
}
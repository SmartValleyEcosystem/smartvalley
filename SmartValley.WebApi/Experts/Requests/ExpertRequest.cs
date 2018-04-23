using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Experts.Requests
{
    public class ExpertRequest
    {
        public string TransactionHash { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }
    }
}
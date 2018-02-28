using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Experts.Requests
{
    public class UpdateExpertRequest
    {
        public string TransactionHash { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }
    }
}
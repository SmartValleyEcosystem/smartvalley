using System.ComponentModel.DataAnnotations;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Admin.Request
{
    public class AdminExpertUpdateRequest : ExpertUpdateRequest
    {
        [Required]
        public string Address { get; set; }

        public string Email { get; set; }
    }
}

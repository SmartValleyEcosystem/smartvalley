using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Admin.Request
{
    public class AdminSetAvailabilityRequest: SetAvailabilityRequest
    {
        [Required]
        public Address Address { get; set; }
    }
}

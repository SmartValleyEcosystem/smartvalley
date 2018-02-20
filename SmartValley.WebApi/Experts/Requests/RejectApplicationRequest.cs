using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Experts.Requests
{
    public class RejectApplicationRequest
    {
        [Required]
        public string Reason { get; set; }
    }
}
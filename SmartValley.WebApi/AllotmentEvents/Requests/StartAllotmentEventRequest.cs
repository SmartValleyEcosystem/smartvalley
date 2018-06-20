using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class StartAllotmentEventRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class UpdateAllotmentEventRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}
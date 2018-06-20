using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class PublishAllotmentEventRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}
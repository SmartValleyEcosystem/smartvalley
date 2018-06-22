using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class PlaceAllotmentEventBidRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}

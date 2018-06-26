using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class ReceiveShareRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}
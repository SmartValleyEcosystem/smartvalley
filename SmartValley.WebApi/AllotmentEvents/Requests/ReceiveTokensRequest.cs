using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class ReceiveTokensRequest
    {
        [Required]
        public string TransactionHash { get; set; }
    }
}
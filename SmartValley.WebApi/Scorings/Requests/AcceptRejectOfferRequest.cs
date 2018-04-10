using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class AcceptRejectOfferRequest
    {
        [Required]
        public string TransactionHash { get; set; }

        [Required]
        public long ScoringId { get; set; }

        [Required]
        public long AreaId { get; set; }
    }
}
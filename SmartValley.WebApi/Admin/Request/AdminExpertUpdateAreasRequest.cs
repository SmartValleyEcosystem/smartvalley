using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Admin.Request
{
    public class AdminExpertUpdateAreasRequest
    {
        [Required]
        public string Address { get; set; }

        public string TransactionHash { get; set; }
    }
}

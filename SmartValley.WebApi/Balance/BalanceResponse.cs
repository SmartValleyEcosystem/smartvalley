using System.Threading.Tasks;

namespace SmartValley.WebApi.User
{
    public class BalanceResponse
    {
        public bool HadReceiviedEther { get; set; }
        public double Balance { get; set; }
    }
}
using SmartValley.Domain.Contracts;

namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class TokenBalanceResponse
    {
        public string TokenAddress { get; set; }
        
        public string HolderAddress { get; set; }
        
        public string Balance { get; set; }
        
        public static TokenBalanceResponse Create(TokenBalance tokenBalance)
        {
            return new TokenBalanceResponse
                   {
                       TokenAddress = tokenBalance.TokenHolder.TokenAddress,
                       HolderAddress = tokenBalance.TokenHolder.HolderAddress,
                       Balance = tokenBalance.Balance.ToString()
                   };
        }
    }
}
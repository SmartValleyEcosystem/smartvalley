using System.Numerics;

namespace SmartValley.Domain.Contracts
{
    public class TokenBalance
    {
        public BigInteger Balance { get; }
        
        public TokenHolder TokenHolder { get; }

        public TokenBalance(TokenHolder tokenHolder, BigInteger balance)
        {
            Balance = balance;
            TokenHolder = tokenHolder;
        }
    }
}
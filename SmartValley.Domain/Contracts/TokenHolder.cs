using SmartValley.Domain.Exceptions;

namespace SmartValley.Domain.Contracts
{
    public class TokenHolder
    {
        public string TokenAddress { get; }
        
        public string HolderAddress { get; }

        public TokenHolder(string tokenAddress, string holderAddress)
        {
            TokenAddress = tokenAddress ?? throw new AppErrorException(ErrorCode.IncorrectData);
            HolderAddress = holderAddress ?? throw new AppErrorException(ErrorCode.IncorrectData);
        }
    }
}
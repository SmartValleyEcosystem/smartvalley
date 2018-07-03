using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IERC223ContractClient
    {
        Task<IReadOnlyCollection<TokenBalance>> GetTokensBalancesAsync(IReadOnlyCollection<TokenHolder> tokenHolderAddresses);
    }
}
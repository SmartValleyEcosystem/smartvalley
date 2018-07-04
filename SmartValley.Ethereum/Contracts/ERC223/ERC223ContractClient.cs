using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.ERC223
{
    public class ERC223ContractClient : IERC223ContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAbi;

        public ERC223ContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<IReadOnlyCollection<TokenBalance>> GetTokensBalancesAsync(IReadOnlyCollection<TokenHolder> tokenHolders)
        {
            return await Task.WhenAll(tokenHolders.Select(GetTokenBalanceAsync));
        }

        private async Task<TokenBalance> GetTokenBalanceAsync(TokenHolder tokenHolder)
        {
            var balance = await _contractClient.CallFunctionAsync<BigInteger>(tokenHolder.TokenAddress, _contractAbi, "balanceOf", tokenHolder.HolderAddress);
            return new TokenBalance(tokenHolder, balance);
        }
    }
}
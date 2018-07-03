using System.Collections.Generic;
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
            var balances = new List<TokenBalance>();
            foreach (var tokenHolder in tokenHolders)
            {
                var balance = await GetTokenBalanceAsync(tokenHolder.TokenAddress, tokenHolder.HolderAddress);
                balances.Add(new TokenBalance(tokenHolder, balance));
            }

            return balances;
        }

        private Task<BigInteger> GetTokenBalanceAsync(string tokenAddress, string holderAddress)
            => _contractClient.CallFunctionAsync<BigInteger>(tokenAddress, _contractAbi, "balanceOf", holderAddress);
    }
}
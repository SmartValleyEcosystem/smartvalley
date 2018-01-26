using System.Threading.Tasks;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Application.Contracts
{
    public class TokenContractClient : ITokenContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        private int? _decimals;

        public TokenContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<int> GetDecimalsAsync()
        {
            if (!_decimals.HasValue)
                _decimals = await _contractClient.CallFunctionAsync<int>(_contractAddress, _contractAbi, "decimals");
            return _decimals.Value;
        }
    }
}
using System.Threading.Tasks;
using SmartValley.Application.Extensions;

namespace SmartValley.Application.Contracts
{
    public class VotingManagerContractClient : IVotingManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;
        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public VotingManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<string> GetLastSprintAddressAsync()
        {
            var sprintAddress = await _contractClient.CallFunctionAsync<string>(_contractAddress, _contractAbi, "lastSprint");
            return sprintAddress.IsAddressEmpty() ? null : sprintAddress;
        }
    }
}

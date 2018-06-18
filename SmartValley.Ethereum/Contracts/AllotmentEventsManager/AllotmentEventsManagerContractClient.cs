using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.AllotmentEventsManager
{
    public class AllotmentEventsManagerContractClient : IAllotmentEventsManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public AllotmentEventsManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public Task<string> GetAllotmentEventContractAddressAsync(long eventId)
        {
            return _contractClient.CallFunctionAsync<string>(
                _contractAddress,
                _contractAbi,
                "getAllotmentEventContractAddress",
                eventId);
        }
    }
}
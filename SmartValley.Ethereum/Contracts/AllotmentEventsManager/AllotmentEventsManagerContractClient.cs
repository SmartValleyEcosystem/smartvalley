using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
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

        public async Task<bool> IsDeletedAsync(long eventId)
        {
            var address = await GetAllotmentEventContractAddressAsync(eventId);
            return address.IsEmpty();
        }

        public async Task<Address> GetAllotmentEventContractAddressAsync(long eventId)
        {
            return await _contractClient.CallFunctionAsync<string>(
                _contractAddress,
                _contractAbi,
                "getAllotmentEventContractAddress",
                eventId);
        }
    }
}
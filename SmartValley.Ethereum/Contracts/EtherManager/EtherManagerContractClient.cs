using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Exceptions;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.EtherManager
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EtherManagerContractClient : IEtherManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public EtherManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;

            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public Task<bool> HasReceivedEtherAsync(string address)
            => _contractClient.CallFunctionAsync<bool>(_contractAddress, _contractAbi, "receiversMap", address);

        public async Task<string> SendEtherToAsync(string address)
        {
            if (await HasReceivedEtherAsync(address))
                throw new EtherAlreadySentException(address);

            return await _contractClient.SignAndSendTransactionAsync(_contractAddress, _contractAbi, "giftEth", address);
        }
    }
}
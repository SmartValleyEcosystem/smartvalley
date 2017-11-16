using System.Threading.Tasks;
using SmartValley.Application.Exceptions;

namespace SmartValley.Application.Contracts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EtherManagerContractClient : EthereumContractClient, IEtherManagerContractClient
    {
        public EtherManagerContractClient(NethereumOptions nethereumOptions)
            : base(nethereumOptions.RpcAddress, nethereumOptions.EtherManagerContract)
        {
        }

        public Task<bool> HasReceivedEtherAsync(string address)
            => GetFunction("receiversMap").CallAsync<bool>(address);

        public async Task SendEtherToAsync(string address)
        {
            if (await HasReceivedEtherAsync(address))
                throw new EtherAlreadySentException(address);

            await SignAndSendTransactionAsync("giftEth", address);
        }
    }
}
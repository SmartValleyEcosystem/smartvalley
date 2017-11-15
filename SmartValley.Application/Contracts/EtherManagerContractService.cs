using System;
using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EtherManagerContractService : EthereumContractClient, IEtherManagerContractService
    {
        public EtherManagerContractService(NethereumOptions nethereumOptions)
            : base(nethereumOptions.RpcAddress, nethereumOptions.EtherManagerContract)
        {
        }

        public Task<bool> HasReceivedEtherAsync(string address)
            => GetFunction("receiversMap").CallAsync<bool>(address);

        public async Task SendEtherToAsync(string address)
        {
            if (await HasReceivedEtherAsync(address))
                //throw new EtherAlreadySentException(address);
                throw new InvalidOperationException();

            await SignAndSendTransactionAsync("giftEth", address);
        }
    }
}
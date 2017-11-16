using System.Threading.Tasks;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Web3;
using SmartValley.Application.Contracts;

namespace SmartValley.Application
{
    public class EthereumClient
    {
        private readonly Web3 _web3; //TODO Initialize Web3 in Startup?

        public EthereumClient(NethereumOptions options)
        {
            _web3 = InitializeWeb3(options.RpcAddress);
        }

        public async Task<double> GetBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return (double) Web3.Convert.FromWei(balance.Value);
        }

        private static Web3 InitializeWeb3(string rpcAddress)
        {
            if (!string.IsNullOrEmpty(rpcAddress))
                return new Web3(rpcAddress);

            var ipcClient = new IpcClient("./geth.ipc");
            return new Web3(ipcClient);
        }
    }
}
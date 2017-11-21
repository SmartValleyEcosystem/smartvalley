using System.Threading.Tasks;
using Nethereum.Web3;

namespace SmartValley.Application
{
    public class EthereumClient
    {
        private readonly Web3 _web3;

        public EthereumClient(Web3 web3) => _web3 = web3;

        public async Task<double> GetBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return (double) Web3.Convert.FromWei(balance.Value);
        }
    }
}
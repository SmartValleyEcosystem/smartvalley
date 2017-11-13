using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;

namespace SmartValley.WebApi.EtherSending
{
    public class EtherSendingService
    {
        private readonly string _contractOwner;
        private readonly string _contractAddress;
        private readonly string _contractAbi;
        private readonly string _password;

        private readonly Web3 _web3;

        public EtherSendingService(
            IOptions<Web3Options> web3Configuration,
            IOptions<ContractOptions> contractConfiguration)
            : this(contractConfiguration.Value.Owner,
                   contractConfiguration.Value.Address,
                   contractConfiguration.Value.Abi,
                   contractConfiguration.Value.Password,
                   web3Configuration.Value.NodeAddress)
        {
        }

        public EtherSendingService(
            string contractOwner,
            string contractAddress,
            string contractAbi,
            string password,
            string nodeAddress)
        {
            _contractOwner = contractOwner;
            _contractAddress = contractAddress;
            _contractAbi = contractAbi;
            _password = password;
            _web3 = InitializeWeb3(nodeAddress);
        }

        public Task<bool> HadReceivedEtherAsync(string address)
            => GetFunction("receiversMap").CallAsync<bool>(address);

        public async Task<double> GetEtherBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return (double) Web3.Convert.FromWei(balance.Value);
        }

        public async Task SendEtherToAsync(string address)
        {
            var giftEtherFunction = GetFunction("giftEth");
            var transactionInput = await CreateTransactionInput(giftEtherFunction, functionInput: address);
            var transactionHash = await _web3.Personal.SignAndSendTransaction.SendRequestAsync(transactionInput, _password);
            await WaitForConfirmationAsync(transactionHash);
        }

        private async Task<TransactionInput> CreateTransactionInput(Function function, params object[] functionInput)
        {
            var estimatedGasHex = await function.EstimateGasAsync(functionInput);
            var gas = new HexBigInteger(estimatedGasHex.Value * 110 / 100); // Adding 10% just in case
            var gasPrice = new HexBigInteger(Web3.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei));

            return function.CreateTransactionInput(_contractOwner, gas, gasPrice, null, functionInput);
        }

        private Function GetFunction(string functionName)
        {
            var contract = _web3.Eth.GetContract(_contractAbi, _contractAddress);
            return contract.GetFunction(functionName);
        }

        private async Task WaitForConfirmationAsync(string transactionHash)
        {
            while (await GetReceiptAsync(transactionHash) == null)
                await Task.Delay(1000);
        }

        private Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
            => _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

        private static Web3 InitializeWeb3(string nodeAddress)
        {
            if (!string.IsNullOrEmpty(nodeAddress))
                return new Web3(nodeAddress);

            var ipcClient = new IpcClient("./geth.ipc");
            return new Web3(ipcClient);
        }
    }
}
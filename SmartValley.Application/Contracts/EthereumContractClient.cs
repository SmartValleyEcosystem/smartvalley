using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;

namespace SmartValley.Application.Contracts
{
    public abstract class EthereumContractClient
    {
        private const int TransactionReceiptPollingIntervalMilliseconds = 1000;
        private const int AdditionalGasPercent = 10;

        private readonly string _contractAbi;
        private readonly string _contractAddress;
        private readonly string _contractOwner;
        private readonly string _password;

        private readonly Web3 _web3;

        protected EthereumContractClient(string rpcAddress, ContractOptions contractOptions)
        {
            _contractAbi = contractOptions.Abi;
            _contractAddress = contractOptions.Address;
            _contractOwner = contractOptions.Owner;
            _password = contractOptions.Password;

            _web3 = InitializeWeb3(rpcAddress);
        }

        protected Function GetFunction(string functionName)
        {
            var contract = _web3.Eth.GetContract(_contractAbi, _contractAddress);
            return contract.GetFunction(functionName);
        }

        protected async Task SignAndSendTransactionAsync(string functionName, params object[] functionInput)
        {
            var function = GetFunction(functionName);
            var transactionInput = await CreateTransactionInputAsync(function, functionInput);
            var transactionHash = await _web3.Personal.SignAndSendTransaction.SendRequestAsync(transactionInput, _password);

            await WaitForConfirmationAsync(transactionHash);
        }

        protected async Task SendRawTransactionAsync(string signedTransactionData)
        {
            var transactionHash = await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransactionData);
            await WaitForConfirmationAsync(transactionHash);
        }
        
        private async Task WaitForConfirmationAsync(string transactionHash)
        {
            while (await GetReceiptAsync(transactionHash) == null)
                await Task.Delay(TransactionReceiptPollingIntervalMilliseconds);
        }

        private async Task<TransactionInput> CreateTransactionInputAsync(Function function, params object[] functionInput)
        {
            var estimatedGasHex = await function.EstimateGasAsync(functionInput);
            var gas = new HexBigInteger(estimatedGasHex.Value * (100 + AdditionalGasPercent) / 100);
            var gasPrice = new HexBigInteger(Web3.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei));

            return function.CreateTransactionInput(_contractOwner, gas, gasPrice, new HexBigInteger(0), functionInput);
        }

        private Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
            => _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

        private static Web3 InitializeWeb3(string rpcAddress)
        {
            if (!string.IsNullOrEmpty(rpcAddress))
                return new Web3(rpcAddress);

            var ipcClient = new IpcClient("./geth.ipc");
            return new Web3(ipcClient);
        }
    }
}
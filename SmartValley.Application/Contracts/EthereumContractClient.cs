using System;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;

namespace SmartValley.Application.Contracts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumContractClient
    {
        private const int TransactionReceiptPollingIntervalMilliseconds = 1000;
        private const int AdditionalGasPercent = 10;

        private readonly string _contractOwner;
        private readonly string _password;

        private readonly Web3 _web3;

        public EthereumContractClient(Web3 web3, NethereumOptions nethereumOptions)
        {
            _contractOwner = nethereumOptions.Owner;
            _password = nethereumOptions.Password;
            _web3 = web3;
        }

        public Task<TResult> CallFunctionAsync<TResult>(
            string address,
            string abi,
            string functionName,
            params object[] functionInput)
        {
            return GetFunction(address, abi, functionName).CallAsync<TResult>(functionInput);
        }

        public async Task<string> SignAndSendTransactionAsync(
            string contractAddress,
            string contractAbi,
            string functionName,
            params object[] functionInput)
        {
            var function = GetFunction(contractAddress, contractAbi, functionName);
            var transactionInput = await CreateTransactionInputAsync(function, functionInput);
            return await _web3.Personal.SignAndSendTransaction.SendRequestAsync(transactionInput, _password);
        }

        public async Task WaitForConfirmationAsync(string transactionHash)
        {
            while (await GetReceiptAsync(transactionHash) == null)
                await Task.Delay(TransactionReceiptPollingIntervalMilliseconds);
        }

        private Function GetFunction(string address, string abi, string functionName)
        {
            var contract = _web3.Eth.GetContract(abi, address);
            return contract.GetFunction(functionName);
        }

        private async Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt?.Status != null && receipt.Status.Value != 1)
                throw new InvalidOperationException($"Transaction '{transactionHash}' failed.");

            return receipt;
        }

        private async Task<TransactionInput> CreateTransactionInputAsync(Function function, params object[] functionInput)
        {
            var estimatedGasHex = await function.EstimateGasAsync(functionInput);
            var gas = new HexBigInteger(estimatedGasHex.Value * (100 + AdditionalGasPercent) / 100);
            var gasPrice = new HexBigInteger(Web3.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei));

            return function.CreateTransactionInput(_contractOwner, gas, gasPrice, new HexBigInteger(0), functionInput);
        }
    }
}
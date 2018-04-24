using System;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace SmartValley.Application
{
    public class EthereumClient
    {
        private const int TransactionReceiptPollingIntervalMilliseconds = 1000;

        private readonly Web3 _web3;

        public EthereumClient(Web3 web3) => _web3 = web3;

        public async Task<double> GetBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return (double) Web3.Convert.FromWei(balance.Value);
        }

        public async Task WaitForConfirmationAsync(string transactionHash)
        {
            while (await GetReceiptAsync(transactionHash) == null)
                await Task.Delay(TransactionReceiptPollingIntervalMilliseconds);
        }

        private async Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt?.Status != null && receipt.Status.Value != 1)
                throw new InvalidOperationException($"Transaction '{transactionHash}' failed.");

            return receipt;
        }
    }
}
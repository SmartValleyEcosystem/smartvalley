﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace SmartValley.Ethereum
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumClient
    {
        private const int TransactionReceiptPollingIntervalMilliseconds = 1000;
        private const int DefaultTransactionConfirmationsCount = 12;

        private readonly int _expectedConfirmationsCount;
        private readonly Web3 _web3;

        public EthereumClient(Web3 web3, NethereumOptions nethereumOptions)
        {
            _web3 = web3;
            _expectedConfirmationsCount = nethereumOptions.TransactionConfirmationsCount ?? DefaultTransactionConfirmationsCount;
        }

        public async Task<string> GetBalanceAsync(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return balance.Value.ToString(CultureInfo.InvariantCulture);
        }

        public async Task WaitForConfirmationAsync(string transactionHash)
        {
            while (!await IsConfirmedAsync(transactionHash))
                await Task.Delay(TransactionReceiptPollingIntervalMilliseconds);
        }

        public async Task<TransactionInfo> GetTransactionInfoAsync(string transactionHash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt?.Status == null)
                return TransactionInfo.NotMined();

            var gasUsed = (long) receipt.CumulativeGasUsed.Value;
            if (receipt.Status.Value != 1)
                return TransactionInfo.Failed(gasUsed);

            var currentBlockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var confirmationsCount = currentBlockNumber.Value - receipt.BlockNumber;
            return confirmationsCount >= _expectedConfirmationsCount
                       ? TransactionInfo.Completed(gasUsed)
                       : TransactionInfo.NotConfirmed();
        }

        private async Task<bool> IsConfirmedAsync(string transactionHash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt?.Status != null && receipt.Status.Value != 1)
                throw new InvalidOperationException($"Transaction '{transactionHash}' failed.");

            return receipt != null;
        }
    }
}
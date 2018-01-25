﻿using System.Threading.Tasks;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Application.Contracts
{
    public class TokenContractClient : ITokenContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public TokenContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public Task<int> GetDecimalsAsync()
        {
            return _contractClient.CallFunctionAsync<int>(_contractAddress, _contractAbi, "decimals");
        }
    }
}
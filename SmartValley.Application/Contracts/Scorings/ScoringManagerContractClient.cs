using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Extensions;

namespace SmartValley.Application.Contracts.Scorings
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringManagerContractClient : IScoringManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public ScoringManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<string> GetScoringAddressAsync(string projectIdString)
        {
            var projectId = projectIdString.Replace("-", "").HexToBigInteger(false);

            var scoringAddress = await _contractClient.CallFunctionAsync<string>(_contractAddress, _contractAbi, "scoringsMap", projectId);

            if (scoringAddress.IsAddressEmpty())
                throw new InvalidOperationException("Project id was not found in contract.");

            return scoringAddress;
        }
    }
}
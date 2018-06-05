using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Ethereum.Contracts.ScoringsRegistry.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;
using SmartValley.Ethereum.Extensions;

namespace SmartValley.Ethereum.Contracts.ScoringsRegistry
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringsRegistryContractClient : IScoringsRegistryContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public ScoringsRegistryContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<Address> GetScoringAddressAsync(Guid projectExternalId)
        {
            Address scoringAddress = await _contractClient.CallFunctionAsync<string>(_contractAddress, _contractAbi, "getScoringAddressById", projectExternalId.ToBigInteger());

            if (scoringAddress.IsEmpty())
                throw new InvalidOperationException("Project id was not found in contract.");

            return scoringAddress;
        }

        public async Task<IReadOnlyCollection<AreaExpertsCount>> GetRequiredExpertsCountsAsync(Guid projectExternalId)
        {
            var requiredExpertsCounts = await _contractClient.CallFunctionAsync<AreaExpertsCountDto>(_contractAddress, _contractAbi, "getRequiredExpertsCounts", projectExternalId.ToBigInteger());

            return requiredExpertsCounts.Areas.Select((a, i) => new AreaExpertsCount {Area = (AreaType) a, Count = requiredExpertsCounts.Counts[i]}).ToArray();
        }
    }
}
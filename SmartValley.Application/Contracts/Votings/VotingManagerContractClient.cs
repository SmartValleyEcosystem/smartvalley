using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Extensions;

namespace SmartValley.Application.Contracts.Votings
{
    public class VotingManagerContractClient : IVotingManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        private readonly int _votingSprintDurationInDays;

        public VotingManagerContractClient(EthereumContractClient contractClient, VotingManagerContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
            _votingSprintDurationInDays = contractOptions.VotingSprintDurationInDays;
        }

        public async Task<IReadOnlyCollection<Guid>> GetProjectsQueueAsync()
        {
            var externalIds = await _contractClient.CallFunctionAsync<List<BigInteger>>(_contractAddress, _contractAbi, "getProjectsQueue");
            return externalIds.Select(e => e.ToGuid()).ToArray();
        }

        public Task<uint> GetMinimumVotingProjectsCountAsync()
            => _contractClient.CallFunctionAsync<uint>(_contractAddress, _contractAbi, "minimumProjectsCount");

        public Task<string> CreateSprintAsync()
            => _contractClient.SignAndSendTransactionAsync(_contractAddress, _contractAbi, "createSprint", _votingSprintDurationInDays);

        public async Task<string> GetLastSprintAddressAsync()
        {
            var sprintAddress = await _contractClient.CallFunctionAsync<string>(_contractAddress, _contractAbi, "lastSprint");
            return sprintAddress.IsAddressEmpty() ? null : sprintAddress;
        }
    }
}
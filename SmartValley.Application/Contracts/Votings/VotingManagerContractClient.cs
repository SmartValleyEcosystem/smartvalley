using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Extensions;
using SmartValley.Application.Contracts.Votings.Dto;

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
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ProjectsQueueDto>(_contractAddress, _contractAbi, "getProjectsQueue");
            return dto.ProjectsQueue.Select(e => e.ToGuid()).ToArray();
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

        public async Task<IReadOnlyCollection<string>> GetAllSprintsAddressesAsync()
        {
            var sprintsDto = await _contractClient.CallFunctionDeserializingToObjectAsync<VotingSprintsDto>(_contractAddress, _contractAbi, "getSprints");
            return sprintsDto.SprintsAddresses;
        }
    }
}
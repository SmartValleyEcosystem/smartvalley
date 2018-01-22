using System;
using System.Linq;
using System.Threading.Tasks;
using IcoLab.Common;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.Votings.Dto;
using SmartValley.Application.Extensions;
using SmartValley.Domain;

namespace SmartValley.Application.Contracts.Votings
{
    public class VotingSprintContractClient : IVotingSprintContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAbi;

        public VotingSprintContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<VotingSprintDetails> GetDetailsAsync(string sprintAddress)
        {
            if (sprintAddress.IsAddressEmpty())
                throw new InvalidOperationException("Sprint address can not be empty.");

            var sprintDto = await _contractClient.CallFunctionDeserializingToObjectAsync<VotingSprintDto>(sprintAddress, _contractAbi, "getSprint");

            return new VotingSprintDetails
                   {
                       AcceptanceThreshold = sprintDto.AcceptanceThreshold,
                       EndDate = DateUtils.FromUnixTime(sprintDto.EndDate),
                       MaximumScore = sprintDto.MaximumScore,
                       ProjectExternalIds = sprintDto.ProjectExternalIds.Select(e => e.ToGuid()).ToList(),
                       StartDate = DateUtils.FromUnixTime(sprintDto.StartDate),
                       Address = sprintAddress
                   };
        }

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            var investorVotes = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _contractAbi, "getInvestorVotes", investorAddress);
            return new InvestorVotes
                   {
                       ProjectExternalIds = investorVotes.ProjectExternalIds,
                       TokenAmount = investorVotes.TokenAmount
                   };
        }

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
        {
            return _contractClient.CallFunctionAsync<long>(sprintAddress, _contractAbi, "getVote", investorAddress, projectId);
        }
    }
}
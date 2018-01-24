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
        private readonly ITokenContractClient _tokenContractClient;

        private readonly string _contractAbi;

        public VotingSprintContractClient(EthereumContractClient contractClient, ContractOptions contractOptions, ITokenContractClient tokenContractClient)
        {
            _contractClient = contractClient;
            _tokenContractClient = tokenContractClient;
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
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _contractAbi, "getInvestorVotes", investorAddress);
            return new InvestorVotes
                   {
                       ProjectExternalIds = dto.ProjectExternalIds.Select(p => p.ToGuid()).ToArray(),
                       TokenAmount = dto.TokenAmount.FromWei(await _tokenContractClient.GetDecimalsAsync())
                   };
        }

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
        {
            return _contractClient.CallFunctionAsync<long>(sprintAddress, _contractAbi, "getVote", investorAddress, projectId);
        }
    }
}
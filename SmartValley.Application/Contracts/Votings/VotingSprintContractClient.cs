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

            var sprintDto = await _contractClient.CallFunctionDeserializingToObjectAsync<VotingSprintDto>(sprintAddress, _contractAbi, "getDetails");

            return new VotingSprintDetails(
                sprintAddress,
                DateUtils.FromUnixTime(sprintDto.StartDate),
                DateUtils.FromUnixTime(sprintDto.EndDate),
                sprintDto.AcceptanceThreshold,
                sprintDto.MaximumScore.FromWei(await _tokenContractClient.GetDecimalsAsync()),
                sprintDto.ProjectExternalIds.Select(e => e.ToGuid()).ToArray(),
                sprintDto.Number);
        }

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _contractAbi, "getInvestorVotes", investorAddress);
            return new InvestorVotes
                   {
                       ProjectsExternalIds = dto.ProjectExternalIds.Select(p => p.ToGuid()).ToArray(),
                       TokenAmount = dto.TokenAmount.FromWei(await _tokenContractClient.GetDecimalsAsync())
                   };
        }

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
            => _contractClient.CallFunctionAsync<long>(sprintAddress, _contractAbi, "getVote", investorAddress, projectId.ToBigInteger());

        public Task<bool> IsAcceptedAsync(string sprintAddress, Guid projectId)
            => _contractClient.CallFunctionAsync<bool>(sprintAddress, _contractAbi, "isAccepted", projectId.ToBigInteger());
    }
}
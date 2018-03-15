using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
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
            var decimals = await _tokenContractClient.GetDecimalsAsync();

            return new VotingSprintDetails(
                sprintAddress,
                DateTimeOffset.FromUnixTimeSeconds(sprintDto.StartDate),
                DateTimeOffset.FromUnixTimeSeconds(sprintDto.EndDate),
                sprintDto.AcceptanceThreshold,
                sprintDto.MaximumScore.FromWei(decimals),
                sprintDto.ProjectExternalIds.Select(e => e.ToGuid()).ToArray(),
                sprintDto.Number);
        }

        public async Task<double> GetProjectTotalTokensAsync(string sprintAddress, Guid projectExternalId)
        {
            var result = await _contractClient.CallFunctionAsync<BigInteger>(sprintAddress, _contractAbi, "projectTokenAmounts", projectExternalId.ToBigInteger());
            return result.FromWei(await _tokenContractClient.GetDecimalsAsync());
        }

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _contractAbi, "getInvestorVotes", investorAddress);
            var decimals = await _tokenContractClient.GetDecimalsAsync();
            return new InvestorVotes(dto.TokenAmount.FromWei(decimals), dto.ProjectExternalIds.Select(p => p.ToGuid()).ToArray());
        }

        public async Task<double> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
        {
            var result = await _contractClient.CallFunctionAsync<BigInteger>(sprintAddress, _contractAbi, "getVote", investorAddress, projectId.ToBigInteger());
            var decimals = await _tokenContractClient.GetDecimalsAsync();
            return result.FromWei(decimals);
        }

        public Task<bool> IsAcceptedAsync(string sprintAddress, Guid projectId)
            => _contractClient.CallFunctionAsync<bool>(sprintAddress, _contractAbi, "isAccepted", projectId.ToBigInteger());
    }
}
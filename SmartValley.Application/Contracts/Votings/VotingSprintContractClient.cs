using System;
using System.Linq;
using System.Threading.Tasks;
using IcoLab.Common;
using Org.BouncyCastle.Utilities.Date;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.Votings.Dto;
using SmartValley.Application.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Interfaces;

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

            var sprintDto = await _contractClient.CallFunctionDeserializingToObjectAsync<VotingSprintDto>(sprintAddress, _contractAbi, "getDetails");

            return new VotingSprintDetails(
                sprintAddress, 
                DateTimeUtilities.UnixMsToDateTime(sprintDto.StartDate),
                DateTimeUtilities.UnixMsToDateTime(sprintDto.EndDate), 
                sprintDto.AcceptanceThreshold, 
                sprintDto.MaximumScore, 
                sprintDto.ProjectExternalIds.Select(e => e.ToGuid()).ToArray());

        }

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _contractAbi, "getInvestorVotes", investorAddress);
            return new InvestorVotes
                   {
                       ProjectsExternalIds = dto.ProjectExternalIds.Select(e => e.ToGuid()).ToArray(),
                       TokenAmount = dto.TokenAmount
                   };
        }

        public Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
            => _contractClient.CallFunctionAsync<long>(sprintAddress, _contractAbi, "getVote", investorAddress, projectId.ToBigInteger());

        public Task<bool> IsAcceptedAsync(string sprintAddress, Guid projectId)
            => _contractClient.CallFunctionAsync<bool>(sprintAddress, _contractAbi, "isAccepted", projectId.ToBigInteger());
    }
}
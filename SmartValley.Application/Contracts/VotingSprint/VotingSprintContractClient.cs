using System;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using SmartValley.Application.Contracts.VotingSprint.Dto;
using SmartValley.Application.Extensions;
using SmartValley.Domain;

namespace SmartValley.Application.Contracts.VotingSprint
{
    public class VotingSprintContractClient : IVotingSprintContractClient
    {
        private readonly EthereumContractClient _contractClient;
        private readonly string _abi;

        public VotingSprintContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _abi = contractOptions.Abi;
        }

        private static Guid StringToGuid(string hex)
        {
            if (hex.Length < 32)
                hex = hex.PadLeft(32, '0');
            return new Guid(hex);
        }

        public async Task<Sprint> GetSprintByAddress(string sprintAddress)
        {
            if (sprintAddress.IsAddressEmpty())
                throw new InvalidOperationException("Sprint address can not be empty.");

            var sprintDto = await _contractClient.CallFunctionDeserializingToObjectAsync<SprintDto>(sprintAddress, _abi, "getSprintInformation");

            return new Sprint
            {
                AcceptanceThreshold = sprintDto.AcceptanceThreshold,
                EndDate = DateTimeOffset.FromUnixTimeMilliseconds(sprintDto.EndDate * 1000).DateTime,
                MaximumScore = sprintDto.MaximumScore,
                ProjectExternalIds = sprintDto.ProjectExternalIds.Select(e => StringToGuid(e.ToHex(false).Replace("0x", ""))).ToList(),
                StartDate = DateTimeOffset.FromUnixTimeMilliseconds(sprintDto.StartDate * 1000).DateTime
            };
        }

        public async Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress)
        {
            var investor = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorVotesDto>(sprintAddress, _abi, "getInvestorVotesInformation", investorAddress);
            return new InvestorVotes
            {
                ProjectExternalIds = investor.ProjectExternalIds,
                TokenAmount = investor.TokenAmount
            };
        }

        public async Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId)
        {
            var investorProjectVotes = await _contractClient.CallFunctionDeserializingToObjectAsync<InvestorProjectVoteDto>(sprintAddress, _abi, "getVote", investorAddress, projectId);
            return investorProjectVotes.TokenAmount;
        }
    }
}

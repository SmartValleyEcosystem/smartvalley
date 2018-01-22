using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.Scorings.Dto;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Scorings
{
    public class ScoringContractClient : IScoringContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAbi;

        public ScoringContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<IReadOnlyCollection<EstimateScore>> GetEstimatesAsync(string scoringAddress)
        {
            var estimates = await _contractClient.CallFunctionDeserializingToObjectAsync<EstimatesDto>(scoringAddress, _contractAbi, "getEstimates");
            return estimates
                   .Questions
                   .Select((questionId, i) => new EstimateScore(questionId, estimates.Scores[i], estimates.Experts[i]))
                   .ToArray();
        }

        public async Task<ProjectScoringStatistics> GetScoringStatisticsAsync(string scoringAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ScoringStatisticsDto>(scoringAddress, _contractAbi, "getScoringInformation");
            return new ProjectScoringStatistics
                   {
                       Score = dto.IsScored ? dto.Score : (int?) null,
                       IsScoredByHr = dto.IsScoredByHr,
                       IsScoredByAnalyst = dto.IsScoredByAnalyst,
                       IsScoredByTech = dto.IsScoredByTech,
                       IsScoredByLawyer = dto.IsScoredByLawyer
                   };
        }

        public Task<uint> GetRequiredSubmissionsInAreaCountAsync(string scoringAddress)
        {
            return _contractClient.CallFunctionAsync<uint>(scoringAddress, _contractAbi, "REQUIRED_SUBMISSIONS_IN_AREA");
        }
    }
}
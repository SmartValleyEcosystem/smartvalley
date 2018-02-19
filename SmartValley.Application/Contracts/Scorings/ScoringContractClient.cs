using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ScoringStatisticsDto>(scoringAddress, _contractAbi, "getResults");
            var scoredAreas = dto.Areas
                                 .Where((t, i) => dto.AreaResults[i])
                                 .Cast<AreaType>()
                                 .ToList();

            return new ProjectScoringStatistics(dto.IsScored ? dto.Score : (int?) null, scoredAreas);
        }

        public Task<uint> GetRequiredSubmissionsInAreaCountAsync(string scoringAddress, AreaType areaType)
        {
            return _contractClient.CallFunctionAsync<uint>(scoringAddress, _contractAbi, "getRequiredSubmissionsInArea", (int) areaType);
        }
    }
}
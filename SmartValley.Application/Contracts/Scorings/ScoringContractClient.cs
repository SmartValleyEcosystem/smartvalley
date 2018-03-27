using System;
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
        private const int ScorePrecision = 2;

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
                   .ScoringCriteria
                   .Select((scoringCriterionId, i) => new EstimateScore(scoringCriterionId, (Score) estimates.Scores[i], estimates.Experts[i]))
                   .ToArray();
        }

        public async Task<ProjectScoringStatistics> GetScoringStatisticsAsync(string scoringAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ScoringStatisticsDto>(scoringAddress, _contractAbi, "getResults");

            var areaScores = new Dictionary<AreaType, double?>();
            for (var i = 0; i < dto.Areas.Count; i++)
            {
                var area = (AreaType) dto.Areas[i];
                var isCompleted = dto.AreaResults[i];
                var score = dto.AreaScores[i] / Math.Pow(10, ScorePrecision);

                areaScores[area] = isCompleted ? score : (double?) null;
            }

            return new ProjectScoringStatistics(dto.IsScored ? dto.Score : (int?) null, areaScores);
        }
    }
}
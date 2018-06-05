using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Ethereum.Contracts.Scoring.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.Scoring
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

        public async Task<ScoringResults> GetResultsAsync(string scoringAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ScoringResultsDto>(scoringAddress, _contractAbi, "getResults");

            var areaScores = new Dictionary<AreaType, double>();
            for (var i = 0; i < dto.Areas.Count; i++)
            {
                var area = (AreaType) dto.Areas[i];
                var areaScore = dto.AreaScores[i] / Math.Pow(10, ScorePrecision);

                areaScores[area] = areaScore;
            }

            var score = dto.Score / Math.Pow(10, ScorePrecision);
            return new ScoringResults(score, areaScores);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Project.Dto;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Project
{
    public class ProjectContractClient : IProjectContractClient
    {
        private readonly EthereumContractClient _contractClient;
        private readonly string _abi;

        public ProjectContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _abi = contractOptions.Abi;
        }

        public async Task<IReadOnlyCollection<EstimateScore>> GetEstimatesAsync(string projectAddress)
        {
            var estimates = await _contractClient.CallFunctionDeserializingToObjectAsync<EstimatesDto>(projectAddress, _abi, "getEstimates");
            return estimates
                .Questions
                .Select((questionId, i) => new EstimateScore(questionId, estimates.Scores[i], estimates.Experts[i]))
                .ToArray();
        }

        public async Task<ProjectScoringStatistics> GetScoringStatisticsAsync(string projectAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<ScoringStatisticsDto>(projectAddress, _abi, "getScoringInformation");
            return new ProjectScoringStatistics
                   {
                       Score = dto.IsScored ? dto.Score : (int?) null,
                       IsScoredByHr = dto.IsScoredByHr,
                       IsScoredByAnalyst = dto.IsScoredByAnalyst,
                       IsScoredByTech = dto.IsScoredByTech,
                       IsScoredByLawyer = dto.IsScoredByLawyer,
                   };
        }
    }
}
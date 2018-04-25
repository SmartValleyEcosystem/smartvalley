using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringContractClient
    {
        Task<IReadOnlyCollection<EstimateScore>> GetEstimatesAsync(string scoringAddress);

        Task<ProjectScoringStatistics> GetScoringStatisticsAsync(string scoringAddress);
    }
}
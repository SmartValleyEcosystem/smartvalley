using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Project
{
    public interface IProjectContractClient
    {
        Task<IReadOnlyCollection<EstimateScore>> GetEstimatesAsync(string projectAddress);

        Task<ProjectScoringStatistics> GetScoringStatisticsAsync(string projectAddress);

        Task<uint> GetRequiredSubmissionsInAreaCountAsync(string projectAddress);
    }
}
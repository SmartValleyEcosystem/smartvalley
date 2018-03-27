using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringRepository
    {
        Task<int> AddAsync(Scoring scoring);

        Task<Scoring> GetByIdAsync(long id);

        Task<int> UpdateWholeAsync(Scoring scoring);

        Task<Scoring> GetByProjectIdAsync(long projectId);

        Task AddAreasAsync(IReadOnlyCollection<AreaScoring> areaScorings);

        Task<IReadOnlyCollection<ScoringProjectDetails>> GetScoringProjectsDetailsByScoringIdsAsync(IReadOnlyCollection<long> scoringIds);

        Task<IReadOnlyCollection<ScoringAreaStatistics>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset tillDate);

        Task<bool> HasEnoughExpertsAsync(long scoringId);

        Task<double?> GetAreaScoreAsync(long scoringId, AreaType areaType);

        Task SetAreaScoreAsync(long scoringId, AreaType area, double score);
    }
}
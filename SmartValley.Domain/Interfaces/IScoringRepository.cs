﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringRepository
    {
        void Add(Scoring scoring);

        Task<Scoring> GetByProjectIdAsync(long projectId);

        Task<IReadOnlyCollection<ScoringProjectDetails>> GetScoringProjectsDetailsByScoringIdsAsync(IReadOnlyCollection<long> scoringIds);

        Task<IReadOnlyCollection<ScoringAreaStatistics>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset tillDate);

        Task SaveChangesAsync();

        Task<Scoring> GetByIdAsync(long scoringId);
    }
}
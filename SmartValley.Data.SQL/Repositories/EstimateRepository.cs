using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class EstimateRepository : EntityCrudRepository<Estimate>, IEstimateRepository
    {
        public EstimateRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId)
            => await ReadContext.Estimates.Where(e => e.ProjectId == projectId).ToArrayAsync();

        public async Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ExpertiseArea category)
            => await ReadContext.Estimates.Where(e => e.ProjectId == projectId && e.ScoringCategory == category).ToArrayAsync();

        public async Task<IReadOnlyCollection<long>> GetProjectsEstimatedByExpertAsync(string expertAddress, ExpertiseArea category)
        {
            return await ReadContext
                       .Estimates
                       .Where(e => e.ExpertAddress.Equals(expertAddress, StringComparison.OrdinalIgnoreCase) && e.ScoringCategory == category)
                       .Select(e => e.ProjectId)
                       .Distinct()
                       .ToArrayAsync();
        }
    }
}
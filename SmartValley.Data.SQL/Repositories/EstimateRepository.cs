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

        public async Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ExpertiseArea expertiseArea)
            => await ReadContext.Estimates.Where(e => e.ProjectId == projectId && e.ScoringCategory == expertiseArea).ToArrayAsync();

        public async Task<IReadOnlyCollection<long>> GetProjectsEstimatedByExpertAsync(string expertAddress, ExpertiseArea expertiseArea)
        {
            return await ReadContext
                       .Estimates
                       .Where(e => e.ExpertAddress.Equals(expertAddress, StringComparison.OrdinalIgnoreCase) && e.ScoringCategory == expertiseArea)
                       .Select(e => e.ProjectId)
                       .Distinct()
                       .ToArrayAsync();
        }
    }
}
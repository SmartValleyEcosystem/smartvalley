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

        public async Task<IReadOnlyCollection<Estimate>> GetByProjectAsync(long projectId)
            => await ReadContext.Estimates.Where(e => e.ProjectId == projectId).ToArrayAsync();

        public async Task<IReadOnlyCollection<Estimate>> GetByProjectIdAndCategoryAsync(long projectId, ScoringCategory category)
        {
            return await ReadContext.Estimates.Where(e => e.ProjectId == projectId && e.ScoringCategory == category).ToArrayAsync();
        }
    }
}
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ScoringRepository : EntityCrudRepository<Scoring>, IScoringRepository
    {
        public ScoringRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => ReadContext.Scorings.FirstAsync(scoring => scoring.ProjectId == projectId);
    }
}
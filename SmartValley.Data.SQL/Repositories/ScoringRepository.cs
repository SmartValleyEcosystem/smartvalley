using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringRepository : EntityCrudRepository<Scoring>, IScoringRepository
    {
        public ScoringRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => ReadContext.Scorings.FirstOrDefaultAsync(scoring => scoring.ProjectId == projectId);

        public async Task<bool> IsCompletedInAreaAsync(long scoringId, AreaType areaType)
        {
            var areaScoring = await ReadContext.AreaScorings.FirstOrDefaultAsync(s => s.ScoringId == scoringId && s.AreaId == areaType);
            return areaScoring?.IsCompleted == true;
        }

        public async Task SetAreasCompletedAsync(long scoringId, IReadOnlyCollection<AreaType> areas)
        {
            foreach (var area in areas)
            {
                var areaScoring = new AreaScoring {ScoringId = scoringId, AreaId = area, IsCompleted = true};
                EditContext.AreaScorings.Attach(areaScoring).Property(s => s.IsCompleted).IsModified = true;
            }

            await EditContext.SaveAsync();
        }

        public Task AddAreasAsync(IReadOnlyCollection<AreaScoring> areaScorings)
        {
            EditContext.AreaScorings.AddRange(areaScorings);
            return EditContext.SaveAsync();
        }
    }
}
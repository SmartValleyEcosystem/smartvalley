using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimateCommentRepository : EntityCrudRepository<EstimateComment>, IEstimateCommentRepository
    {
        public EstimateCommentRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<EstimateComment>> GetByScoringIdAsync(long scoringId, AreaType areaType)
        {
            return await (from estimate in ReadContext.EstimateComments
                          join scoringCriterion in ReadContext.ScoringCriteria on estimate.ScoringCriterionId equals scoringCriterion.Id
                          where scoringCriterion.AreaType == areaType && estimate.ScoringId == scoringId
                          select estimate)
                       .ToArrayAsync();
        }
    }
}
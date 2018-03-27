using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ScoringCriterionRepository : EntityCrudRepository<ScoringCriterion>, IScoringCriterionRepository
    {
        public ScoringCriterionRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }
    }
}
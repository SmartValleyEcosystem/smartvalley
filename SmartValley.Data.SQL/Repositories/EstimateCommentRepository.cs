using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class EstimateCommentRepository : EntityCrudRepository<EstimateComment>, IEstimateCommentRepository
    {
        public EstimateCommentRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<EstimateComment>> GetAsync(long projectId, ExpertiseArea expertiseArea)
        {
            return await (from estimate in ReadContext.Estimates
                          join question in ReadContext.Questions on estimate.QuestionId equals question.Id
                          where estimate.ProjectId == projectId && question.ExpertiseArea == expertiseArea
                          select estimate)
                       .ToArrayAsync();
        }
    }
}
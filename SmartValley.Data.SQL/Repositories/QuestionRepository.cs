using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class QuestionRepository : EntityCrudRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Dictionary<long, IReadOnlyCollection<Estimate>>> GetQuestionWithEstimatesAsync(long projectId, ExpertiseArea expertiseArea)
        {
            return (from estimate in ReadContext.Estimates
                    join question in ReadContext.Questions on estimate.QuestionId equals question.Id
                    where estimate.ProjectId == projectId && question.ExpertiseArea == expertiseArea
                    group estimate by estimate.QuestionId
                    into g
                    select new {QuesionId = g.Key, Estimates = (IReadOnlyCollection<Estimate>) g.ToArray()})
                .ToDictionaryAsync(t => t.QuesionId, t => t.Estimates);
        }
    }
}
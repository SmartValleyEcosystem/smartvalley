using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
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

        public async Task<IList<ScoringCriterionPrompt>> GetScoringCriterionPromptsAsync(long scoringApplicationId, AreaType areaType)
        {
            return await (from answer in ReadContext.ScoringApplicationAnswers
                          join question in ReadContext.ScoringApplicationQuestions on answer.QuestionId equals question.Id
                          join mapping in ReadContext.ScoringCriteriaMappings on question.Id equals mapping.ScoringApplicationQuestionId
                          join criterion in ReadContext.ScoringCriteria on mapping.ScoringCriterionId equals criterion.Id
                          where answer.ScoringApplicationId == scoringApplicationId
                                && criterion.AreaType == areaType
                          select new ScoringCriterionPrompt
                                 {
                                     CriterionId = criterion.Id,
                                     Title = question.Key,
                                     Answer = answer.Value,
                                     PromptType = ScoringCriterionPromptType.Plain,
                                     QuestionControlType = question.Type
                          }).ToListAsync();
        }
    }
}
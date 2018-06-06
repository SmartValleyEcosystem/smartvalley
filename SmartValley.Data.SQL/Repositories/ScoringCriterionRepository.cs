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
    public class ScoringCriterionRepository : IScoringCriterionRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public ScoringCriterionRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<IReadOnlyCollection<ScoringCriterion>> GetAsync()
        {
            return await _readContext.ScoringCriteria.ToArrayAsync();
        }

        public async Task<IList<ScoringCriterionPrompt>> GetScoringCriterionPromptsAsync(long scoringApplicationId, AreaType areaType)
        {
            return await (from answer in _readContext.ScoringApplicationAnswers
                          join question in _readContext.ScoringApplicationQuestions on answer.QuestionId equals question.Id
                          join mapping in _readContext.ScoringCriteriaMappings on question.Id equals mapping.ScoringApplicationQuestionId
                          join criterion in _readContext.ScoringCriteria on mapping.ScoringCriterionId equals criterion.Id
                          where answer.ScoringApplicationId == scoringApplicationId
                                && criterion.AreaType == areaType
                          orderby question.Order
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
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringCriterionRepository
    {
        Task<IReadOnlyCollection<ScoringCriterion>> GetAllAsync();

        Task<IList<ScoringCriterionPrompt>> GetScoringCriterionPromptsAsync(long scoringApplicationId, AreaType areaType);
    }
}
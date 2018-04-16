using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class CriterionPromptResponse
    {
        public long ScoringCriterionId { get; set; }

        public IReadOnlyCollection<PromptResponse> Prompts { get; set; }

        public static CriterionPromptResponse Create(long criterionId, IReadOnlyCollection<ScoringCriterionPrompt> promts)
        {
            return new CriterionPromptResponse
                   {
                       ScoringCriterionId = criterionId,
                       Prompts = promts.Select(PromptResponse.Create).ToArray()
                   };
        }
    }
}
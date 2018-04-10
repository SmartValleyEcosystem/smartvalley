using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringAreaConslusionResponse
    {
        public string Conslusion { get; set; }

        public static ScoringAreaConslusionResponse FromDomain(ExpertScoringConclusion conslusion)
        {
            return new ScoringAreaConslusionResponse
                   {
                       Conslusion = conslusion.Conclusion
                   };
        }
    }
}
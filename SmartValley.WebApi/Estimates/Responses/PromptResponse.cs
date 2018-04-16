using SmartValley.Domain;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.ScoringApplications.Responses;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class PromptResponse
    {
        public string Title { get; set; }

        public string Answer { get; set; }

        public IReadOnlyCollection<ProjectTeamMemberResponse> ProjectTeamMembers { get; set; }

        public IReadOnlyCollection<AdviserResponse> ProjectAdvisers { get; set; }

        public SocialNetworks SocialNetworks { get; set; }

        public ScoringCriterionPromptType Type { get; set; }

        public static PromptResponse Create(ScoringCriterionPrompt scoringCriterionPrompt)
        {
            return new PromptResponse
                   {
                       Title = scoringCriterionPrompt.Title,
                       Answer = scoringCriterionPrompt.Answer,
                       Type = scoringCriterionPrompt.Type,
                       ProjectTeamMembers = scoringCriterionPrompt.TeamMembers.Select(ProjectTeamMemberResponse.Create).ToArray(),
                       ProjectAdvisers = scoringCriterionPrompt.Advisers.Select(AdviserResponse.Create).ToArray(),
                       SocialNetworks = scoringCriterionPrompt.SocialNetworks
                   };
        }
    }
}
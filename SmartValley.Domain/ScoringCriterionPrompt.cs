using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringCriterionPrompt
    {
        public long CriterionId { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public IReadOnlyCollection<ScoringApplicationTeamMember> TeamMembers { get; set; } = new List<ScoringApplicationTeamMember>();

        public IReadOnlyCollection<ScoringApplicationAdviser> Advisers { get; set; } = new List<ScoringApplicationAdviser>();

        public SocialNetworks SocialNetworks { get; set; }

        public ScoringCriterionPromptType Type { get; set; }
    }
}
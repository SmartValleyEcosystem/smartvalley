using System;

namespace SmartValley.Domain
{
    public class InvestorProjectVote
    {
        public Guid ProjectExternalId { get; }

        public double InvestorTokenVote { get; }

        public double TotalTokenVote { get; }

        public InvestorProjectVote(Guid projectExternalId, double investorTokenVote, double totalTokenVote)
        {
            ProjectExternalId = projectExternalId;
            InvestorTokenVote = investorTokenVote;
            TotalTokenVote = totalTokenVote;
        }
    }
}

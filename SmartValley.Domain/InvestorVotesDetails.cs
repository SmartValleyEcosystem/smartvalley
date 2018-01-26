using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class InvestorVotesDetails
    {
        public double TokenAmount { get; set; }
        public IReadOnlyCollection<InvestorProjectVote> InvestorProjectVotes { get; set; }
    }
}

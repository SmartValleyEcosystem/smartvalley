using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class InvestorVotesDetails
    {
        public double InvestorVoteBalance { get; }

        public IReadOnlyCollection<InvestorProjectVote> InvestorProjectVotes { get; }

        public InvestorVotesDetails(double investorVoteBalance, IReadOnlyCollection<InvestorProjectVote> investorProjectVotes)
        {
            InvestorVoteBalance = investorVoteBalance;
            InvestorProjectVotes = investorProjectVotes;
        }
    }
}

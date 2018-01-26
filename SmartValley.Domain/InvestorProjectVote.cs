using System;

namespace SmartValley.Domain
{
    public class InvestorProjectVote
    {
        public Guid ProjectExternalId { get; set; }

        public double InvestorTokenVote { get; set; }

        public double TotalTokenVote { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class InvestorVotes
    {
        public double TokenAmount { get; set; }

        public IReadOnlyCollection<Guid> ProjectsExternalIds { get; set; }
    }
}

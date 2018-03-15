using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class InvestorVotes
    {
        public double TokenAmount { get; }

        public IReadOnlyCollection<Guid> ProjectsExternalIds { get; }

        public InvestorVotes(double tokenAmount, IReadOnlyCollection<Guid> projectsExternalIds)
        {
            TokenAmount = tokenAmount;
            ProjectsExternalIds = projectsExternalIds;
        }
    }
}

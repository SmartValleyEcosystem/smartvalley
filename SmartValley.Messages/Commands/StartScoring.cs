using System.Collections.Generic;

namespace SmartValley.Messages.Commands
{
    public class StartScoring
    {
        public string TransactionHash { get; set; }

        public long ProjectId { get; set; }

        public IReadOnlyCollection<AreaExpertsCount> ExpertCounts { get; set; }
    }
}
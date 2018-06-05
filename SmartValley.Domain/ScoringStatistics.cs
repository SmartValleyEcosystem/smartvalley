using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class ScoringStatistics
    {
        public DateTimeOffset AcceptingDeadline { get; set; }
        public DateTimeOffset ScoringDeadline { get; set; }

        public IReadOnlyCollection<ScoringStatisticsInArea> ScoringStatisticsInArea { get; set; }
    }
}
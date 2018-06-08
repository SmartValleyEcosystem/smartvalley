using System;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringReport
    {
        public DateTimeOffset AcceptingDeadline { get; set; }
        public DateTimeOffset ScoringDeadline { get; set; }
        
        public IReadOnlyCollection<Expert> Experts { get; set; }
        public IReadOnlyCollection<ScoringReportInArea> ScoringReportsInAreas { get; set; }
    }
}
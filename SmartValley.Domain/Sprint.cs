using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class Sprint
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long AcceptanceThreshold { get; set; }

        public long MaximumScore { get; set; }

        public List<Guid> ProjectExternalIds { get; set; }
    }
}

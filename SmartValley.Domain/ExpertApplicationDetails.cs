using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertApplicationDetails
    {
        public ExpertApplication ExpertApplication { get; set; }

        public IReadOnlyCollection<Area> Areas { get; set; }
    }
}
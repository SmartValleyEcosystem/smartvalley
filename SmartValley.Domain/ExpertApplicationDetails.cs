using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertApplicationDetails
    {
        public Address Address { get; set; }

        public ExpertApplication ExpertApplication { get; set; }
        
        public IReadOnlyCollection<Area> Areas { get; set; }
    }
}
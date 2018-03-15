using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertApplicationDetails
    {
        public Address Address { get; }

        public ExpertApplication ExpertApplication { get; }
        
        public IReadOnlyCollection<Area> Areas { get; }

        public ExpertApplicationDetails(Address address, ExpertApplication expertApplication, IReadOnlyCollection<Area> areas)
        {
            Address = address;
            ExpertApplication = expertApplication;
            Areas = areas;
        }
    }
}
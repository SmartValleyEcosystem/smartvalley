using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertDetails
    {
        public Address Address { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public IReadOnlyCollection<Area> Areas { get; set; }
    }
}

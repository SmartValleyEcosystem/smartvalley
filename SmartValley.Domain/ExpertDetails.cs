using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertDetails
    {
        public Address Address { get; }

        public string Email { get; }

        public string Name { get; }

        public string About { get; }

        public bool IsAvailable { get; }

        public IReadOnlyCollection<Area> Areas { get; }

        public ExpertDetails(Address address, string email, string name, string about, bool isAvailable, IReadOnlyCollection<Area> areas)
        {
            Address = address;
            Email = email;
            About = about;
            IsAvailable = isAvailable;
            Areas = areas;
            Name = name;
        }
    }
}

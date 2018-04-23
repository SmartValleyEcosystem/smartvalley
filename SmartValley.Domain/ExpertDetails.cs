using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertDetails
    {
        public Address Address { get; }

        public string Email { get; }

        public string FirstName { get; }

        public string SecondName { get; }

        public string About { get; }

        public bool IsAvailable { get; }

        public IReadOnlyCollection<Area> Areas { get; }

        public ExpertDetails(Address address, string email, string firstName, string secondName, string about, bool isAvailable, IReadOnlyCollection<Area> areas)
        {
            Address = address;
            Email = email;
            IsAvailable = isAvailable;
            Areas = areas;
            About = about;
            FirstName = firstName;
            SecondName = secondName;
        }
    }
}

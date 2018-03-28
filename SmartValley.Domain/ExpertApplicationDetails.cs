using System.Collections.Generic;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ExpertApplicationDetails
    {
        public ExpertApplicationDetails(
            Address address,
            string email,
            ExpertApplication expertApplication,
            IReadOnlyCollection<Area> areas)
        {
            Address = address;
            Email = email;
            ExpertApplication = expertApplication;
            Areas = areas;
        }

        public Address Address { get; }

        public string Email { get; }

        public ExpertApplication ExpertApplication { get; }

        public IReadOnlyCollection<Area> Areas { get; }
    }
}
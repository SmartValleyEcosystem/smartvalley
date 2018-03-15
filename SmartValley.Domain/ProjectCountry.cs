using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectCountry
    {
        public long ProjectId { get; }

        public Country Country { get; }

        public ProjectCountry(long projectId, Country country)
        {
            ProjectId = projectId;
            Country = country;
        }
    }
}
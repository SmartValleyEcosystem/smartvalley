using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Person : IEntityWithId
    {
        public long Id { get; set; }

        public long ApplicationId { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public PersonType PersonType { get; set; }

        //public virtual Application Application { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public PersonType PersonType { get; set; }

        public virtual IEnumerable<PersonApplication> Applications { get; set; }
    }
}

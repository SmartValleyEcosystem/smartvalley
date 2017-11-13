using System;

namespace SmartValley.Domain.Entities
{
    public class PersonApplication
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public Guid ApplicationId { get; set; }

        public virtual Person Person { get; set; }

        public virtual Application Application { get; set; }
    }
}

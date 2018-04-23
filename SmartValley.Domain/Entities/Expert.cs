using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class Expert 
    {
        public long UserId { get; set; }

        public bool IsAvailable { get; set; }

        public string About { get; set; }

        public User User { get; set; }

        public IEnumerable<ExpertArea> ExpertAreas { get; set; }
    }
}

using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project: IEntityWithId
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string ProjectArea { get; set; }

        public string ProblemDesc { get; set; }

        public string SolutionDesc { get; set; }
    }
}

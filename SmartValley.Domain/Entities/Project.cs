using System;

namespace SmartValley.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ProjectArea { get; set; }

        public string ProblemDesc { get; set; }

        public string SolutionDesc { get; set; }

        public string ProjectStatus { get; set; }

        public string WhitePaperLink { get; set; }
    }
}

namespace SmartValley.WebApi.Project
{
    public class ScoredProjectSummaryResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string ProjectArea { get; set; }

        public string ProblemDescription { get; set; }

        public string SolutionDescription { get; set; }

        public double Score { get; set; }
    }
}
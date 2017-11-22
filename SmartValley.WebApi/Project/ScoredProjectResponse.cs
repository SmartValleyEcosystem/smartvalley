namespace SmartValley.WebApi.Project
{
    public class ScoredProjectResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string Area { get; set; }

        public string Description { get; set; }

        public double Score { get; set; }
    }
}
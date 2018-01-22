namespace SmartValley.Application.Contracts.Scorings
{
    public class ProjectScoringStatistics
    {
        public int? Score { get; set; }

        public bool IsScoredByHr { get; set; }

        public bool IsScoredByAnalyst { get; set; }

        public bool IsScoredByTech { get; set; }

        public bool IsScoredByLawyer { get; set; }
    }
}
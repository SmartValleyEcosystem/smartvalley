using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Scorings.Dto
{
    [FunctionOutput]
    public class ScoringStatisticsDto
    {
        [Parameter("bool", "_isScored", 1)]
        public bool IsScored { get; set; }

        [Parameter("uint256", "_score", 2)]
        public int Score { get; set; }

        [Parameter("bool", "_isScoredByHr", 3)]
        public bool IsScoredByHr { get; set; }

        [Parameter("bool", "_isScoredByAnalyst", 4)]
        public bool IsScoredByAnalyst { get; set; }

        [Parameter("bool", "_isScoredByTech", 5)]
        public bool IsScoredByTech { get; set; }

        [Parameter("bool", "_isScoredByLawyer", 6)]
        public bool IsScoredByLawyer { get; set; }
    }
}
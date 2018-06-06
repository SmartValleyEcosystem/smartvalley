using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace SmartValley.Ethereum.Contracts.Scoring.Dto
{
    [FunctionOutput]
    public class ScoringResultsDto
    {
        [Parameter("uint256", "_score", 1)]
        public int Score { get; set; }

        [Parameter("uint256[]", "_areas", 2)]
        public List<int> Areas { get; set; }

        [Parameter("uint[]", "_areaScores", 3)]
        public List<long> AreaScores { get; set; }
    }
}
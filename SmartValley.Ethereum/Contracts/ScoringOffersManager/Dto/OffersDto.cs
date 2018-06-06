using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SmartValley.Ethereum.Contracts.ScoringOffersManager.Dto
{
    [FunctionOutput]
    public class OffersDto
    {
        [Parameter("uint256[]", "_areas", 1)]
        public List<int> Areas { get; set; }

        [Parameter("address[]", "_experts", 2)]
        public List<string> Experts { get; set; }

        [Parameter("uint256[]", "_states", 3)]
        public List<long> States { get; set; }

        [Parameter("uint256", "_scoringDeadline", 4)]
        public long ScoringDeadline { get; set; }

        [Parameter("uint256", "_acceptingDeadline", 5)]
        public long AcceptingDeadline { get; set; }
    }
}
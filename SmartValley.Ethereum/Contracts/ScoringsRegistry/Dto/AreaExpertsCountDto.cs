using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Ethereum.Contracts.ScoringsRegistry.Dto
{
    public class AreaExpertsCountDto
    {
        [Parameter("uint256[]", "_counts", 1)]
        public List<int> Counts { get; set; }

        [Parameter("uint256[]", "_areas", 2)]
        public List<int> Areas { get; set; }
    }
}

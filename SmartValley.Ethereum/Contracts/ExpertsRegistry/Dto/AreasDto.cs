using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Ethereum.Contracts.ExpertsRegistry.Dto
{
    [FunctionOutput]
    public class AreasDto
    {
        [Parameter("uint256[]", "_areas", 1)]
        public List<int> Areas { get; set; }
    }
}
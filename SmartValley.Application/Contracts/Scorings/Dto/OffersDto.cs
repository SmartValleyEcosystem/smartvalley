using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Scorings.Dto
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
    }
}
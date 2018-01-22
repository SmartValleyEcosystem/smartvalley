using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Scorings.Dto
{
    [FunctionOutput]
    public class EstimatesDto
    {
        [Parameter("uint256[]", "_questions", 1)]
        public List<long> Questions { get; set; }

        [Parameter("uint256[]", "_scores", 2)]
        public List<int> Scores { get; set; }

        [Parameter("address[]", "_experts", 3)]
        public List<string> Experts { get; set; }
    }
}
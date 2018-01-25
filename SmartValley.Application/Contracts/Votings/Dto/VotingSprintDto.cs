using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Votings.Dto
{
    [FunctionOutput]
    public class VotingSprintDto
    {
        [Parameter("uint", "_startDate", 1)]
        public long StartDate { get; set; }

        [Parameter("uint", "_endDate", 2)]
        public long EndDate { get; set; }

        [Parameter("uint", "_acceptanceThresholdPercent", 3)]
        public int AcceptanceThreshold { get; set; }

        [Parameter("uint", "_maximumScore", 4)]
        public BigInteger MaximumScore { get; set; }

        [Parameter("uint[]", "_projectIds", 5)]
        public List<BigInteger> ProjectExternalIds { get; set; }

        [Parameter("uint", "_number", 6)]
        public int Number { get; set; }
    }
}
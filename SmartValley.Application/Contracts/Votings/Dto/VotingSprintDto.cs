using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Votings.Dto
{
    [FunctionOutput]
    public class VotingSprintDto
    {
        [Parameter("uint", "startDate", 1)]
        public long StartDate { get; set; }

        [Parameter("uint", "endDate", 2)]
        public long EndDate { get; set; }

        [Parameter("uint", "acceptanceThresholdPercent", 3)]
        public long AcceptanceThreshold { get; set; }

        [Parameter("uint", "maximumScore", 4)]
        public long MaximumScore { get; set; }

        [Parameter("uint[]", "projectIds", 5)]
        public List<BigInteger> ProjectExternalIds { get; set; }
    }
}
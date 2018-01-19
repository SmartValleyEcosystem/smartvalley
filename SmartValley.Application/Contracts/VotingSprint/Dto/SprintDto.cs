using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace SmartValley.Application.Contracts.VotingSprint.Dto
{
    [FunctionOutput]
    public class SprintDto
    {
        [Parameter("uint", "startDate", 1)]
        public long StartDate { get; set; }

        [Parameter("uint", "endDate", 2)]
        public long EndDate { get; set; }

        [Parameter("uint", "acceptanceThreshold", 3)]
        public long AcceptanceThreshold { get; set; }

        [Parameter("uint", "maximumScore", 4)]
        public long MaximumScore { get; set; }

        [Parameter("uint[]", "projectIds", 5)]
        public List<BigInteger> ProjectExternalIds { get; set; }
    }
}
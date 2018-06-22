using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Ethereum.Contracts.AllotmentEvent.Dto
{
    [FunctionOutput]
    public class AllotmentEventResultInfoDto
    {
        [Parameter("uint256", "_totalTokensToDistribute", 1)]
        public long TotalTokens { get; set; }

        [Parameter("uint256", "_totalBidsAmount", 2)]
        public long TotalBids { get; set; }

        [Parameter("string[]", "_participants", 3)]
        public List<string> Participants { get; set; }

        [Parameter("uint256[]", "_participantBids", 4)]
        public List<long> ParticipantBids { get; set; }

        [Parameter("uint256[]", "_participantShares", 5)]
        public List<long> ParticipantShares { get; set; }
    }
}

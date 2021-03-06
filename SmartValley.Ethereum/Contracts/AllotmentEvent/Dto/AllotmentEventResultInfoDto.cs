﻿using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Ethereum.Contracts.AllotmentEvent.Dto
{
    [FunctionOutput]
    public class AllotmentEventResultInfoDto
    {
        [Parameter("uint256", "_totalTokensToDistribute", 1)]
        public BigInteger TotalTokens { get; set; }

        [Parameter("uint256", "_totalBidsAmount", 2)]
        public BigInteger TotalBids { get; set; }

        [Parameter("address[]", "_participants", 3)]
        public List<string> Participants { get; set; }

        [Parameter("uint256[]", "_participantBids", 4)]
        public List<BigInteger> ParticipantBids { get; set; }

        [Parameter("uint256[]", "_participantShares", 5)]
        public List<BigInteger> ParticipantShares { get; set; }

        [Parameter("bool[]", "_collectedShares", 6)]
        public List<bool> CollectedShares { get; set; }
    }
}

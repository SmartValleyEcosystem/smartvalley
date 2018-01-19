
using System;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.VotingSprint.Dto
{
    [FunctionOutput]
    public class InvestorVotesDto
    {
        [Parameter("uint", "tokenAmount", 1)]
        public long TokenAmount { get; set; }

        [Parameter("uint[]", "projectsIds", 2)]
        public List<Guid> ProjectExternalIds { get; set; }
    }
}

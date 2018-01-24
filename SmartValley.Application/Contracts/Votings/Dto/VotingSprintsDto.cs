using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Votings.Dto
{
    [FunctionOutput]
    public class VotingSprintsDto
    {
        [Parameter("address[]", "_sprints", 1)]
        public List<string> SprintsAddresses { get; set; }
    }
}

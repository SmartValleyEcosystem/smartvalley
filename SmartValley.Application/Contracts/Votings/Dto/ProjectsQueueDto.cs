using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.Votings.Dto
{
    [FunctionOutput]
    public class ProjectsQueueDto
    {
        [Parameter("uint[]", "_projectsQueue", 1)]
        public List<BigInteger> ProjectsQueue { get; set; }
    }
}
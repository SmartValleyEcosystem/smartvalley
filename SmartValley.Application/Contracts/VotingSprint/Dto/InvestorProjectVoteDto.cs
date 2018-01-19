using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Application.Contracts.VotingSprint.Dto
{
    [FunctionOutput]
    public class InvestorProjectVoteDto
    {
        [Parameter("uint256", "tokenAmount", 1)]
        public long TokenAmount { get; set; }
    }
}

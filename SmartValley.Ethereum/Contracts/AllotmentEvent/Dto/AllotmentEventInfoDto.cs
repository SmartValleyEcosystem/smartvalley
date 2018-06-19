using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SmartValley.Ethereum.Contracts.AllotmentEvent.Dto
{
    [FunctionOutput]
    public class AllotmentEventInfoDto
    {
        [Parameter("string", "_name", 1)]
        public string Name { get; set; }

        [Parameter("uint256", "_status", 2)]
        public int Status { get; set; }

        [Parameter("uint256", "_tokenDecimals", 3)]
        public int TokenDecimals { get; set; }

        [Parameter("string", "_tokenTicker", 4)]
        public string TokenTicker { get; set; }

        [Parameter("address", "_tokenContractAddress", 5)]
        public string TokenContractAddress { get; set; }

        [Parameter("uint256", "_startTimestamp", 6)]
        public long StartTimestamp { get; set; }

        [Parameter("uint256", "_finishTimestamp", 7)]
        public long FinishTimestamp { get; set; }
    }
}
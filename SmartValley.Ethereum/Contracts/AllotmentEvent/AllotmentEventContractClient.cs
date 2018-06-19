using System;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Contracts;
using SmartValley.Ethereum.Contracts.AllotmentEvent.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.AllotmentEvent
{
    public class AllotmentEventContractClient : IAllotmentEventContractClient
    {
        private readonly EthereumContractClient _contractClient;
        private readonly string _contractAbi;

        public AllotmentEventContractClient(EthereumContractClient contractClient, string contractAbi)
        {
            _contractClient = contractClient;
            _contractAbi = contractAbi;
        }

        public async Task<AllotmentEventInfo> GetInfoAsync(string contractAddress)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<AllotmentEventInfoDto>(contractAddress, _contractAbi, "getInfo");
            return new AllotmentEventInfo(
                dto.Name,
                (AllotmentEventStatus) dto.Status,
                dto.TokenContractAddress,
                ToDateTimeOffset(dto.StartTimestamp),
                ToDateTimeOffset(dto.FinishTimestamp),
                dto.TokenDecimals,
                dto.TokenTicker);
        }

        private static DateTimeOffset? ToDateTimeOffset(long unixTimeSeconds)
            => unixTimeSeconds == 0 ? (DateTimeOffset?) null : DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Ethereum.Contracts.ScoringOffersManager.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;
using SmartValley.Ethereum.Extensions;

namespace SmartValley.Ethereum.Contracts.ScoringOffersManager
{
    public class ScoringOffersManagerContractClient : IScoringOffersManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public ScoringOffersManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<ScoringInfo> GetScoringInfoAsync(Guid projectExternalId)
        {
            var offersDto = await _contractClient.CallFunctionDeserializingToObjectAsync<OffersDto>(
                                _contractAddress,
                                _contractAbi,
                                "get",
                                projectExternalId.ToBigInteger());

            return new ScoringInfo(projectExternalId,
                                   offersDto.Experts.Select((e, i) => new ScoringOfferInfo(
                                                      e,
                                                      (AreaType)offersDto.Areas[i],
                                                      (ScoringOfferStatus)offersDto.States[i]
                                                  )).ToArray(),
                                   ToDateTimeOffset(offersDto.AcceptingDeadline),
                                   ToDateTimeOffset(offersDto.ScoringDeadline)
            );
        }

        private static DateTimeOffset ToDateTimeOffset(long unixTimeSeconds)
            => unixTimeSeconds == 0 ? DateTimeOffset.MaxValue : DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
    }
}
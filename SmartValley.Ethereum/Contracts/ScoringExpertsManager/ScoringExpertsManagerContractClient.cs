using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Ethereum.Contracts.ScoringExpertsManager.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;
using SmartValley.Ethereum.Extensions;

namespace SmartValley.Ethereum.Contracts.ScoringExpertsManager
{
    public class ScoringExpertsManagerContractClient : IScoringExpertsManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public ScoringExpertsManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<IReadOnlyCollection<ScoringOfferInfo>> GetOffersAsync(Guid projectExternalId)
        {
            var offersDto = await _contractClient.CallFunctionDeserializingToObjectAsync<OffersDto>(
                                _contractAddress,
                                _contractAbi,
                                "getOffers",
                                projectExternalId.ToBigInteger());

            return offersDto.Experts
                            .Select((e, i) => new ScoringOfferInfo(
                                        projectExternalId,
                                        e,
                                        (AreaType) offersDto.Areas[i],
                                        (ScoringOfferStatus) offersDto.States[i],
                                        DateTimeOffset.FromUnixTimeSeconds(offersDto.ExpirationTimestamp),
                                        ToNullableDate(offersDto.ScoringDeadlines[i])))
                            .ToArray();
        }

        private static DateTimeOffset? ToNullableDate(long unixTimeSeconds)
            => unixTimeSeconds == 0 ? (DateTimeOffset?) null : DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
    }
}
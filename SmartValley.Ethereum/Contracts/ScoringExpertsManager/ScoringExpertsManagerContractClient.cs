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

            return offersDto.Experts.Select((e, i) => CreateOfferInfo(projectExternalId, e, offersDto.Areas[i], offersDto.States[i])).ToArray();
        }

        private static ScoringOfferInfo CreateOfferInfo(Guid projectExternalId, string expertAddress, int area, long offerState)
        {
            return offerState > 2
                       ? new ScoringOfferInfo(projectExternalId, expertAddress, (AreaType) area, ScoringOfferStatus.Pending, DateTimeOffset.FromUnixTimeSeconds(offerState))
                       : new ScoringOfferInfo(projectExternalId, expertAddress, (AreaType) area, (ScoringOfferStatus) offerState, null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.Scorings.Dto;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Extensions;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Scorings
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
            ScoringOfferStatus status;
            DateTimeOffset? timestamp;

            if (offerState > 2)
            {
                status = ScoringOfferStatus.Pending;
                timestamp = DateTimeOffset.FromUnixTimeSeconds(offerState);
            }
            else
            {
                status = offerState == 1 ? ScoringOfferStatus.Accepted : ScoringOfferStatus.Rejected;
                timestamp = null;
            }

            return new ScoringOfferInfo(projectExternalId, expertAddress, (AreaType) area, status, timestamp);
        }
    }
}
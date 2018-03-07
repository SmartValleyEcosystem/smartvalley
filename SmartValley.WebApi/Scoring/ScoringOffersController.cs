﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.WebApi.Scoring.Requests;
using SmartValley.WebApi.Scoring.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Scoring
{
    [Route("api/scoring/offers")]
    [Authorize]
    public class ScoringOffersController : Controller
    {
        private readonly IScoringService _scoringService;
        private readonly EthereumClient _ethereumClient;

        public ScoringOffersController(IScoringService scoringService, EthereumClient ethereumClient)
        {
            _scoringService = scoringService;
            _ethereumClient = ethereumClient;
        }

        [HttpGet("pending")]
        public async Task<CollectionResponse<ScoringOfferResponse>> GetAllPendingAsync()
        {
            var offers = await _scoringService.GetPendingOfferDetailsAsync(User.Identity.Name);

            return new CollectionResponse<ScoringOfferResponse>
                   {
                       Items = offers.Select(ScoringOfferResponse.Create).ToArray()
                   };
        }

        [HttpGet("accepted")]
        public async Task<CollectionResponse<ScoringOfferResponse>> GetAllAcceptedAsync()
        {
            var offers = await _scoringService.GetAcceptedOfferDetailsAsync(User.Identity.Name);

            return new CollectionResponse<ScoringOfferResponse>
                   {
                       Items = offers.Select(ScoringOfferResponse.Create).ToArray()
                   };
        }

        [HttpPut("accept")]
        public async Task<EmptyResponse> AcceptAsync(AcceptRejectOfferRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _scoringService.AcceptOfferAsync(request.ScoringId, request.AreaId, User.Identity.Name);
            return new EmptyResponse();
        }

        [HttpPut("reject")]
        public async Task<EmptyResponse> RejectAsync(AcceptRejectOfferRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _scoringService.RejectOfferAsync(request.ScoringId, request.AreaId, User.Identity.Name);
            return new EmptyResponse();
        }
    }
}
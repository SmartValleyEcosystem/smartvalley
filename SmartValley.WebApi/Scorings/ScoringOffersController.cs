using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.Ethereum;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Scorings.Requests;
using SmartValley.WebApi.Scorings.Responses;

namespace SmartValley.WebApi.Scorings
{
    [Route("api/scoring/offers")]
    [Authorize]
    public class ScoringOffersController : Controller
    {
        private readonly IScoringService _scoringService;
        private readonly EthereumClient _ethereumClient;
        private readonly IClock _clock;

        public ScoringOffersController(
            IScoringService scoringService,
            EthereumClient ethereumClient,
            IClock clock)
        {
            _scoringService = scoringService;
            _ethereumClient = ethereumClient;
            _clock = clock;
        }

        [HttpPut("accept")]
        public async Task<EmptyResponse> AcceptAsync([FromBody] AcceptRejectOfferRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _scoringService.AcceptOfferAsync(request.ScoringId, request.AreaId, User.GetUserId());
            return new EmptyResponse();
        }

        [HttpPut("reject")]
        public async Task<EmptyResponse> RejectAsync([FromBody] AcceptRejectOfferRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _scoringService.RejectOfferAsync(request.ScoringId, request.AreaId, User.GetUserId());
            return new EmptyResponse();
        }

        [HttpGet("status")]
        public async Task<ScoringOfferStatusResponse> GetOfferStatusAsync(GetScoringOfferStatusRequest request)
        {
            var scoring = await _scoringService.GetByProjectIdAsync(request.ProjectId);
            if (scoring == null)
                throw new AppErrorException(ErrorCode.ScoringNotFound);

            var offer = await _scoringService.GetOfferAsync(request.ProjectId, request.AreaType.ToDomain(), User.GetUserId());
            var offerStatus = offer?.Status.ToApi(scoring.AcceptingDeadline, scoring.ScoringDeadline, _clock.UtcNow);
            return new ScoringOfferStatusResponse
                   {
                       Status = offerStatus,
                       Exists = offerStatus.HasValue
                   };
        }

        [HttpGet]
        public async Task<IActionResult> QueryAsync([FromQuery] QueryScoringOffersRequest request)
        {
            var isAdmin = User.IsInRole(nameof(RoleType.Admin));
            if (request.ExpertId.HasValue && request.ExpertId.Value != User.GetUserId() && !isAdmin || !request.ExpertId.HasValue && !isAdmin)
                return Unauthorized();

            var query = new OffersQuery
                        {
                            ExpertId = request.ExpertId,
                            OrderBy = request.OrderBy,
                            SortDirection = request.SortDirection,
                            Statuses = request.Statuses ?? new List<ScoringOfferStatus>(),
                            Offset = request.Offset,
                            Count = request.Count,
                            ScoringId = request.ScoringId,
                            ProjectId = request.ProjectId
                        };
            var now = _clock.UtcNow;
            var offers = await _scoringService.QueryOffersAsync(query, now);

            return Ok(offers.ToPartialCollectionResponse(o => ScoringOfferResponse.Create(o, now)));
        }

        [HttpPut]
        public async Task<EmptyResponse> UpdateOffersAsync([FromBody] UpdateOffersRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _scoringService.UpdateOffersAsync(request.ProjectExternalId);
            return new EmptyResponse();
        }
    }
}
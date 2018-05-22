using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Application.Extensions;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Interfaces;
using SmartValley.Ethereum;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Scorings.Requests;
using SmartValley.WebApi.Scorings.Responses;
using SmartValley.WebApi.WebApi;

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
            var offer = await _scoringService.GetOfferAsync(request.ProjectId, request.AreaType.ToDomain(), User.GetUserId());
            var offerStatus = offer?.Status.ToApi(offer.ExpirationTimestamp, _clock.UtcNow);
            return new ScoringOfferStatusResponse
                   {
                       Status = offerStatus,
                       Exists = offerStatus.HasValue
                   };
        }

        [HttpGet("query")]
        public async Task<PartialCollectionResponse<ScoringOfferResponse>> QueryAsync([FromQuery] QueryScoringOffersRequest request)
        {
            var query = new OffersQuery
                        {
                            ExpertId = User.GetUserId(),
                            OrderBy = request.OrderBy,
                            SortDirection = request.SortDirection,
                            OnlyTimedOut = request.Status == ScoringOfferStatus.Timeout,
                            Status = request.Status?.ToDomain(),
                            Offset = request.Offset,
                            Count = request.Count
                        };
            var now = _clock.UtcNow;
            var offers = await _scoringService.QueryOffersAsync(query, now);

            return offers.ToPartialCollectionResponse(o => ScoringOfferResponse.Create(o, now));
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
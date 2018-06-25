using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.WebApi.AllotmentEvents.Requests;
using SmartValley.WebApi.AllotmentEvents.Responses;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.AllotmentEvents
{
    [Route("api/allotmentEvents")]
    public class AllotmentEventsController : Controller
    {
        private readonly IAllotmentEventService _allotmentEventService;
        private readonly IMessageSession _messageSession;
        private readonly IClock _clock;

        public AllotmentEventsController(IAllotmentEventService allotmentEventService, IMessageSession messageSession, IClock clock)
        {
            _allotmentEventService = allotmentEventService;
            _messageSession = messageSession;
            _clock = clock;
        }

        [HttpGet]
        public async Task<PartialCollectionResponse<AllotmentEventResponse>> GetAsync([FromQuery] QueryAllotmentEventsRequest request)
        {
            var filterStatuses = request.AllotmentEventStatuses ?? new AllotmentEventStatus[0];
            var query = new AllotmentEventsQuery(filterStatuses, request.Offset, request.Count);
            var result = await _allotmentEventService.QueryAsync(query);

            return result.ToPartialCollectionResponse(x => AllotmentEventResponse.Create(x, _clock.UtcNow));
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<CreateAllotmentEventResponse> PostAsync([FromBody] CreateAllotmentEventRequest request)
        {
            var eventId = await _allotmentEventService.CreateAsync(request.Name,
                                                                   request.TokenContractAddress,
                                                                   request.TokenDecimals,
                                                                   request.TokenTicker,
                                                                   request.ProjectId,
                                                                   request.FinishDate);

            return new CreateAllotmentEventResponse(eventId);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpdateAllotmentEventRequest request)
        {
            var command = new UpdateAllotmentEvent
                          {
                              AllotmentEventId = id,
                              TransactionHash = request.TransactionHash,
                              UserId = User.GetUserId()
                          };

            await _messageSession.SendLocal(command);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> Delete(long eventId, string transactionHash)
        {
            await _messageSession.SendLocal(new DeleteAllotmentEvent(User.GetUserId(), eventId, transactionHash ));
            return NoContent();
        }

        [HttpPut("{id}/publish")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> PublishAsync(long id, [FromBody] PublishAllotmentEventRequest request)
        {
            var command = new PublishAllotmentEvent(id, User.GetUserId(), request.TransactionHash);

            await _messageSession.SendLocal(command);
            return NoContent();
        }

        [HttpPut("{id}/start")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> StartAsync(long id, [FromBody] StartAllotmentEventRequest request)
        {
            var command = new StartAllotmentEvent(id, User.GetUserId(), request.TransactionHash);

            await _messageSession.SendLocal(command);
            return NoContent();
        }

        [HttpPut("{id}/participate")]
        [Authorize]
        public async Task<IActionResult> PlaceBidAsync(long id, [FromBody] PlaceAllotmentEventBidRequest request)
        {
            var command = new PlaceAllotmentEventBid(id, User.GetUserId(), request.TransactionHash);

            await _messageSession.SendLocal(command);
            return NoContent();
        }

        [HttpPut("{id}/receiveTokens")]
        [Authorize]
        public async Task<IActionResult> ReceiveTokensAsync(long id, [FromBody] ReceiveTokensRequest request)
        {
            var command = new ReceiveTokens(id, User.GetUserId(), request.TransactionHash);

            await _messageSession.SendLocal(command);
            return NoContent();
        }
    }
}
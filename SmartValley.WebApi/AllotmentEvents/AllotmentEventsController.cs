using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
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

        public AllotmentEventsController(IAllotmentEventService allotmentEventService, IMessageSession messageSession)
        {
            _allotmentEventService = allotmentEventService;
            _messageSession = messageSession;
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<PartialCollectionResponse<AllotmentEventResponse>> GetAsync([FromQuery] QueryAllotmentEventsRequest request)
        {
            var query = new AllotmentEventsQuery(request.AllotmentEventStatuses ?? new AllotmentEventStatus[0], request.Offset, request.Count);
            var result = await _allotmentEventService.QueryAsync(query);

            return result.ToPartialCollectionResponse(AllotmentEventResponse.Create);
        }

        [HttpPut]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateAllotmentEventRequest request)
        {
            var allotmentEvent = await _allotmentEventService.GetByIdAsync(request.AllotmentEventId);

            if (string.IsNullOrWhiteSpace(request.TransactionHash) && allotmentEvent.Status == AllotmentEventStatus.InProgress)
                return BadRequest();

            var command = new UpdateAllotmentEvent
                          {
                              AllotmentEventId = request.AllotmentEventId,
                              TransactionHash = request.TransactionHash,
                              UserId = User.GetUserId(),
                              Name = request.Name,
                              TokenContractAddress = request.TokenContractAddress,
                              TokenDecimals = request.TokenDecimals,
                              TokenTicker = request.TokenTicker,
                              FinishDate = request.FinishDate
                          };

            await _messageSession.SendLocal(command);

            return NoContent();
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

        [HttpPut("{id}/publish")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> PublishAsync(long id, [FromBody] PublishAllotmentEventRequest request)
        {
            var command = new PublishAllotmentEvent(id, User.GetUserId(), request.TransactionHash);

            await _messageSession.SendLocal(command);
            return NoContent();
        }
    }
}
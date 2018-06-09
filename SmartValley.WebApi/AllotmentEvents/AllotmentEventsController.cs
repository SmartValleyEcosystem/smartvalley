using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.WebApi.AllotmentEvents.Requests;
using SmartValley.WebApi.AllotmentEvents.Responses;
using SmartValley.WebApi.Extensions;

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
        public async Task<IActionResult> GetAsync([FromQuery]QueryAllotmentEventsRequest request)
        {
            var query = new AllotmentEventsQuery(request.AllotmentEventStatuses ?? new AllotmentEventStatus[0], request.Offset, request.Count);
            var result = await _allotmentEventService.QueryAsync(query);
            return Ok(result.ToPartialCollectionResponse(AllotmentEventResponse.Create));
        }

        public Task Publish(PublishAllotmentEventRequest publishRequest)
        {
            
        }
    }

    public class PublishAllotmentEventRequest
    {
        public long AllotmentEventId { get; set; }
        public string TransactionHash { get; set; }
    }
}

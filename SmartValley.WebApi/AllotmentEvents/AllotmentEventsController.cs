using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AllotmentEventsController(IAllotmentEventService allotmentEventService)
        {
            _allotmentEventService = allotmentEventService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> GetAsync([FromQuery]QueryAllotmentEventsRequest request)
        {
            var query = new AllotmentEventsQuery(request.AllotmentEventStatuses ?? new AllotmentEventStatus[0], request.Offset, request.Count);
            var result = await _allotmentEventService.QueryAsync(query);
            return Ok(result.ToPartialCollectionResponse(AllotmentEventResponse.Create));
        }
    }
}

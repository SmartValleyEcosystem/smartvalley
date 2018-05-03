using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Feedbacks.Requests;
using SmartValley.WebApi.Feedbacks.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Feedbacks
{
    [Route("api/feedbacks")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feetbackService;

        public FeedbackController(IFeedbackService feetbackService)
        {
            _feetbackService = feetbackService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFeedbackRequest request)
        {
            await _feetbackService.AddAsync(request);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<PartialCollectionResponse<FeedbackResponse>> GetAsync(AllFeedbacksRequest request)
        {
            var feedbacks = await _feetbackService.GetAsync(request.Offset, request.Count);
            var totalCount = await _feetbackService.GetTotalCountAsync();
            return new PartialCollectionResponse<FeedbackResponse>(
                request.Offset, feedbacks.Count, totalCount, feedbacks.Select(FeedbackResponse.Create).ToArray());
        }
    }
}

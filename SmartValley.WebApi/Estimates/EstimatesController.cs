using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly IEstimationService _estimationService;

        public EstimatesController(IEstimationService estimationService)
        {
            _estimationService = estimationService;
        }

        [Authorize]
        public async Task<IActionResult> Post([FromBody] SubmitEstimatesRequest request)
        {
            await _estimationService.SubmitEstimatesAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<GetEstimatesResponse> GetEstimatesAsync(GetEstimatesRequest request)
        {
            var estimates = await _estimationService.GetAsync(request.ProjectId, request.Category);
            var averageScore = _estimationService.CalculateAverageScore(estimates);

            return new GetEstimatesResponse
                   {
                       AverageScore = averageScore,
                       Items = estimates.Select(EstimateResponse.From).ToArray()
                   };
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    [Authorize]
    public class EstimatesController : Controller
    {
        private readonly IEstimationService _estimationService;

        public EstimatesController(IEstimationService estimationService)
        {
            _estimationService = estimationService;
        }

        public Task Post([FromBody] SubmitEstimatesRequest request) => _estimationService.SubmitEstimatesAsync(request);

        [HttpGet]
        public async Task<CollectionResponse<EstimateResponse>> GetEstimatesOfProjectByCategory(GetEstimatesRequest request)
        {
            var estimates = await _estimationService.GetByProjectIdAndCategory(request.ProjectId, request.Category);

            return new CollectionResponse<EstimateResponse>
                   {
                       Items = estimates.Select(EstimateResponse.From).ToArray()
                   };
        }
    }
}
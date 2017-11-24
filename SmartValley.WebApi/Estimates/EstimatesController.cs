using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
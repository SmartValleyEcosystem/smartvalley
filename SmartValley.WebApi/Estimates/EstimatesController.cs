using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly IEstimationService _estimationService;
        private readonly IProjectService _projectService;

        public EstimatesController(IEstimationService estimationService, IProjectService projectService)
        {
            _estimationService = estimationService;
            _projectService = projectService;
        }

        [Authorize]
        public async Task<IActionResult> Post([FromBody] SubmitEstimatesRequest request)
        {
            await _estimationService.SubmitEstimatesAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimatesAsync(GetEstimatesRequest request)
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId);

            if (project.Score == null && !project.AuthorAddress.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            var estimates = await _estimationService.GetAsync(request.ProjectId, request.Category);
            var averageScore = _estimationService.CalculateAverageScore(estimates);

            var response = new GetEstimatesResponse
                           {
                               AverageScore = averageScore,
                               Items = estimates.Select(EstimateResponse.From).ToArray()
                           };

            return Ok(response);
        }
    }
}
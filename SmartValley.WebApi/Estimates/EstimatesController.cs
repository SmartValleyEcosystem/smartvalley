using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Estimates.Requests;
using SmartValley.WebApi.Estimates.Responses;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly EthereumClient _ethereumClient;
        private readonly IEstimationService _estimationService;
        private readonly IProjectService _projectService;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;

        public EstimatesController(
            EthereumClient ethereumClient,
            IEstimationService estimationService,
            IProjectService projectService,
            IScoringCriterionRepository scoringCriterionRepository)
        {
            _ethereumClient = ethereumClient;
            _estimationService = estimationService;
            _projectService = projectService;
            _scoringCriterionRepository = scoringCriterionRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SubmitEstimatesRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _estimationService.SubmitEstimatesAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimatesAsync(GetEstimatesRequest request)
        {
            if (!await _projectService.IsAuthorizedToSeeEstimatesAsync(User.GetUserId(), request.ProjectId))
                return Unauthorized();

            var scoringStatistics = await _estimationService.GetScoringStatisticsInAreaAsync(request.ProjectId, request.AreaType.ToDomain());
            return Ok(GetCriteriaWithEstimatesResponse.Create(scoringStatistics));
        }

        [HttpGet]
        [Route("criteria")]
        public async Task<CollectionResponse<ScoringCriterionResponse>> GetCriteriaAsync()
        {
            var criteria = await _scoringCriterionRepository.GetAllAsync();
            return new CollectionResponse<ScoringCriterionResponse>
                   {
                       Items = criteria.Select(ScoringCriterionResponse.From).ToArray()
                   };
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Interfaces;
using SmartValley.Ethereum;
using SmartValley.WebApi.Estimates.Requests;
using SmartValley.WebApi.Estimates.Responses;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly EthereumClient _ethereumClient;
        private readonly IEstimationService _estimationService;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;
        private readonly IClock _clock;

        public EstimatesController(
            EthereumClient ethereumClient,
            IEstimationService estimationService,
            IScoringCriterionRepository scoringCriterionRepository,
            IClock clock)
        {
            _ethereumClient = ethereumClient;
            _estimationService = estimationService;
            _scoringCriterionRepository = scoringCriterionRepository;
            _clock = clock;
        }

        [Authorize]
        [HttpPost]
        [Route("submit")]
        public async Task<IActionResult> SubmitAsync([FromBody] SubmitEstimateRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _estimationService.SubmitEstimatesAsync(User.GetUserId(), request);
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveEstimatesRequest request)
        {
            await _estimationService.SaveEstimatesAsync(User.GetUserId(), request);
            return NoContent();
        }

        [Authorize]
        [HttpGet]
        [Route("offer")]
        public async Task<ExpertEstimateResponse> GetOfferEstimatesAsync(long projectId, AreaType areaType)
        {
            var expertConclusion = await _estimationService.GetOfferEstimateAsync(User.GetUserId(), projectId, areaType);
            if (expertConclusion == null)
            {
                return ExpertEstimateResponse.Empty;
            }

            return new ExpertEstimateResponse
                   {
                       Conclusion = expertConclusion.Conclusion,
                       Estimates = expertConclusion.Estimates.Select(EstimateResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [CanSeeProject("projectId")]
        public async Task<CollectionResponse<ScoringStatisticsInAreaResponse>> GetEstimatesAsync(long projectId)
        {
            var scoringStatistics = await _estimationService.GetScoringStatisticsAsync(projectId);
            return new CollectionResponse<ScoringStatisticsInAreaResponse>
                   {
                       Items = scoringStatistics.ScoringStatisticsInArea.Select(x => ScoringStatisticsInAreaResponse.Create(x, scoringStatistics.AcceptingDeadline, scoringStatistics.ScoringDeadline, _clock.UtcNow)).ToArray()
                   };
        }

        [HttpGet]
        [Route("criteria")]
        public async Task<CollectionResponse<AreaScoringCriteriaResponse>> GetCriteriaAsync()
        {
            var criteria = await _scoringCriterionRepository.GetAsync();
            return new CollectionResponse<AreaScoringCriteriaResponse>
                   {
                       Items = criteria
                               .GroupBy(c => c.AreaType)
                               .Select(g => AreaScoringCriteriaResponse.Create(g.Key.FromDomain(), g.ToArray()))
                               .ToArray()
                   };
        }

        [HttpGet]
        [Route("project/{projectId}/prompts/{areaType}")]
        public async Task<CollectionResponse<CriterionPromptResponse>> GetCriterionPromptsAsync(CriterionPromptRequest request)
        {
            var prompts = await _estimationService.GetCriterionPromptsAsync(request.ProjectId, request.AreaType.ToDomain());
            return new CollectionResponse<CriterionPromptResponse>
                   {
                       Items = prompts
                               .GroupBy(p => p.CriterionId)
                               .Select(g => CriterionPromptResponse.Create(g.Key, g.ToArray()))
                               .ToArray()
                   };
        }
    }
}
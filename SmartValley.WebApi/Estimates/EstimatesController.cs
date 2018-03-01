using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Estimates.Requests;
using SmartValley.WebApi.Estimates.Responses;
using SmartValley.WebApi.Experts;
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
        private readonly IQuestionRepository _questionRepository;

        public EstimatesController(
            EthereumClient ethereumClient,
            IEstimationService estimationService,
            IProjectService projectService,
            IQuestionRepository questionRepository)
        {
            _ethereumClient = ethereumClient;
            _estimationService = estimationService;
            _projectService = projectService;
            _questionRepository = questionRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubmitEstimatesRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _estimationService.SubmitEstimatesAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimatesAsync(GetEstimatesRequest request)
        {
            if (!await _projectService.IsAuthorizedToSeeEstimatesAsync(User.Identity.Name, request.ProjectId))
                return Unauthorized();

            var scoringStatistics = await _estimationService.GetScoringStatisticsInAreaAsync(request.ProjectId, request.AreaType.ToDomain());
            return Ok(GetQuestionsWithEstimatesResponse.Create(scoringStatistics));
        }

        [HttpGet]
        [Route("questions")]
        public async Task<CollectionResponse<QuestionResponse>> GetQuestionsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return new CollectionResponse<QuestionResponse>
                   {
                       Items = questions.Select(QuestionResponse.From).ToArray()
                   };
        }
    }
}
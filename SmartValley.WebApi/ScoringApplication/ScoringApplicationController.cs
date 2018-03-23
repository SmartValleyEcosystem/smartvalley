using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.ScoringApplication.Requests;
using SmartValley.WebApi.ScoringApplication.Responses;

namespace SmartValley.WebApi.ScoringApplication
{
    [Route("api/projects/{projectId}/scoringapplications")]
    [Authorize]
    public class ScoringApplicationController : Controller
    {
        private readonly IScoringApplicationService _scoringApplicationService;

        public ScoringApplicationController(IScoringApplicationService scoringApplicationService)
        {
            _scoringApplicationService = scoringApplicationService;
        }

        [HttpGet]
        public async Task<ScoringApplicationBlankResponse> GetByProjectIdAsync(long projectId)
        {
            var scoringApplicationQuestions = await _scoringApplicationService.GetQuestionsAsync();
            var scoringApplication = await _scoringApplicationService.GetApplicationAsync(projectId);

            return ScoringApplicationBlankResponse.Create(scoringApplicationQuestions, scoringApplication);
        }

        [HttpPost]
        public async Task SaveAsync(long projectId, [FromBody]SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            await _scoringApplicationService.SaveApplicationAsync(projectId, saveScoringApplicationRequest);
        }

        [HttpPost]
        [Route("submit")]
        public async Task SubmitAsync(long projectId)
        {
            await _scoringApplicationService.SubmitForScoreAsync(projectId);
        }
    }
}
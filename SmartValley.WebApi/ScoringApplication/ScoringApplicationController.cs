using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.ScoringApplication.Requests;
using SmartValley.WebApi.ScoringApplication.Responses;

namespace SmartValley.WebApi.ScoringApplication
{
    [Route("api/projects/{projectId}/scoring/applications")]
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
            var questions = await _scoringApplicationService.GetQuestionsAsync();
            var scoringApplication = await _scoringApplicationService.GetApplicationAsync(projectId);

            if (scoringApplication == null)
                return ScoringApplicationBlankResponse.CreateEmpty(questions);

            return ScoringApplicationBlankResponse.InitializeFromApplication(questions, scoringApplication);
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
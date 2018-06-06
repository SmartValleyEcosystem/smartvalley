using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.ScoringApplications.Requests;
using SmartValley.WebApi.ScoringApplications.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.ScoringApplications
{
    [Route("api/projects/{projectId}/scoring/applications")]
    public class ScoringApplicationController : Controller
    {
        private readonly IScoringApplicationService _scoringApplicationService;
        private readonly IProjectService _projectService;

        public ScoringApplicationController(
            IScoringApplicationService scoringApplicationService,
            IProjectService projectService)
        {
            _scoringApplicationService = scoringApplicationService;
            _projectService = projectService;
        }

        [HttpGet]
        [CanSeeProject("projectId")]
        public async Task<ScoringApplicationResponse> GetByProjectIdAsync(long projectId)
        {
            var questions = await _scoringApplicationService.GetQuestionsAsync();
            var scoringApplication = await _scoringApplicationService.GetApplicationAsync(projectId);

            if (scoringApplication != null)
                return ScoringApplicationResponse.InitializeFromApplication(questions, scoringApplication);

            var project = await _projectService.GetByIdAsync(projectId);
            return ScoringApplicationResponse.CreateEmpty(questions, project);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> SaveAsync(long projectId, [FromBody] SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            var isAuthorizedToEditProjectAsync = await _projectService.IsAuthorizedToEditProjectAsync(projectId, User.GetUserId());
            if (!isAuthorizedToEditProjectAsync)
                return Unauthorized();

            await _scoringApplicationService.SaveAsync(projectId, saveScoringApplicationRequest);
            return NoContent();
        }

        [HttpPut("submit"), Authorize]
        public async Task<IActionResult> SubmitAsync(long projectId)
        {
            var isAuthorizedToEditProjectAsync = await _projectService.IsAuthorizedToEditProjectAsync(projectId, User.GetUserId());
            if (!isAuthorizedToEditProjectAsync)
                return Unauthorized();

            await _scoringApplicationService.SubmitApplicationAsync(projectId);
            return NoContent();
        }
    }
}
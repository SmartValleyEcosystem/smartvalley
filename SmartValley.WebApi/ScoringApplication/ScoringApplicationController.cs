using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.ScoringApplication.Requests;
using SmartValley.WebApi.ScoringApplication.Responses;

namespace SmartValley.WebApi.ScoringApplication
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
        public async Task<ScoringApplicationBlankResponse> GetByProjectIdAsync(long projectId)
        {
            var questions = await _scoringApplicationService.GetQuestionsAsync();
            var scoringApplication = await _scoringApplicationService.GetApplicationAsync(projectId);

            if (scoringApplication == null)
            {
                var project = await _projectService.GetDetailsAsync(projectId);
                return ScoringApplicationBlankResponse.CreateEmpty(questions, project.Project, project.Country, project.TeamMembers);
            }

            return ScoringApplicationBlankResponse.InitializeFromApplication(questions, scoringApplication);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> SaveAsync(long projectId, [FromBody]SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            var isAuthorizedToEditProjectAsync = await _projectService.IsAuthorizedToEditProjectAsync(projectId, User.GetUserId());
            if (!isAuthorizedToEditProjectAsync)
                return Unauthorized();

            await _scoringApplicationService.SaveApplicationAsync(projectId, saveScoringApplicationRequest);
            return NoContent();
        }
    }
}
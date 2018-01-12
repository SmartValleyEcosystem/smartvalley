using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Estimates;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.Scoring.Requests;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Scoring
{
    [Route("api/scoring")]
    [Authorize]
    public class ScoringContoller : Controller
    {
        private readonly IScoringService _scoringService;

        public ScoringContoller(IScoringService scoringService)
        {
            _scoringService = scoringService;
        }

        [HttpGet]
        public async Task<CollectionResponse<ProjectResponse>> GetProjectsForScoringAsync([FromQuery] GetProjectsForScoringRequest request)
        {
            var projects = await _scoringService.GetProjectsForScoringAsync(request.ExpertiseArea.ToDomain(), User.Identity.Name);
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [Route("myprojects")]
        public async Task<CollectionResponse<ProjectResponse>> GetMyProjectsAsync()
        {
            var projects = await _scoringService.GetProjectsByAuthorAsync(User.Identity.Name);
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.Create).ToArray()
                   };
        }
    }
}
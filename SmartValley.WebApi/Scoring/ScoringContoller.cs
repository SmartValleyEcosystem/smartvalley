using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;
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
        public async Task<CollectionResponse<ProjectResponse>> GetProjectForScorring([FromQuery] GetProjecsForScorringRequest request)
        {
            var projects = await _scoringService.GetProjectsForScoringByCategoryAsync(request.Category);
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.From).ToArray()
                   };
        }

        [HttpGet]
        [Route("myprojects")]
        public async Task<CollectionResponse<ProjectResponse>> GetMyProjects()
        {
            var projects = await _scoringService.GetProjectsByAuthorAddressAsync(User.Identity.Name);
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.From).ToArray()
                   };
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IEnumerable<ProjectForScorringResponse>> GetProjectForScorring([FromQuery] ProjecsForScorringRequest request)
        {
            var projects = await _scoringService.GetProjectsForScoringByCategory(request.Category);
            return projects.Select(ProjectForScorringResponse.From);
        }
    }
}
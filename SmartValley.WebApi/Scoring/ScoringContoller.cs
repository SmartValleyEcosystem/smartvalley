using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.WebApi.Estimates;
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

        public ScoringContoller(IScoringService scoringService, EthereumClient ethereumClient)
        {
            _scoringService = scoringService;
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartAsync(StartProjectScoringRequest request)
        {
            await _scoringService.StartAsync(request.ProjectExternalId, request.TransactionHash);
            return NoContent();
        }
    }
}
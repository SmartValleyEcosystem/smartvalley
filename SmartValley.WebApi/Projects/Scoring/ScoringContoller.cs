using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Projects.Scoring.Requests;

namespace SmartValley.WebApi.Projects.Scoring
{
    [Route("api/scoring")]
    [Authorize]
    public class ScoringContoller : Controller
    {
        private readonly IScoringService _scoringService;
        private readonly EthereumClient _ethereumClient;

        public ScoringContoller(IScoringService scoringService, EthereumClient ethereumClient)
        {
            _scoringService = scoringService;
            _ethereumClient = ethereumClient;
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartAsync([FromBody] StartProjectScoringRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

            await _scoringService.StartAsync(request.ProjectExternalId, request.Areas);

            return NoContent();
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.Messages.Commands;
using SmartValley.WebApi.Scorings.Requests;
using SmartValley.WebApi.Scorings.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Scorings
{
    [Route("api/scoring")]
    [Authorize]
    public class ScoringContoller : Controller
    {
        private readonly IScoringService _scoringService;
        private readonly IMessageSession _messageSession;
        private readonly IClock _clock;

        public ScoringContoller(IScoringService scoringService, IMessageSession messageSession, IClock clock)
        {
            _scoringService = scoringService;
            _messageSession = messageSession;
            _clock = clock;
        }

        [HttpGet]
        [CanSeeProject("projectId")]
        public async Task<ScoringResponse> GetByProjectIdAsync(long projectId)
        {
            var scoring = await _scoringService.GetByProjectIdAsync(projectId);
            return ScoringResponse.FromScoring(scoring, _clock.UtcNow);
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartAsync([FromBody] StartProjectScoringRequest request)
        {
            var command = new StartScoring
                          {
                              ProjectId = request.ProjectId,
                              TransactionHash = request.TransactionHash,
                              ExpertCounts = request.Areas
                                             .Select(a => new AreaExpertsCount
                                                          {
                                                              AreaType = (int) a.Area,
                                                              ExpertsCount = a.ExpertsCount
                                                          })
                                             .ToArray()
                          };

            await _messageSession.SendLocal(command);

            return NoContent();
        }

        [HttpPost, Authorize(Roles = nameof(RoleType.Admin))]
        [Route("finish")]
        public async Task FinishAsync([FromBody] UpdateScoringRequest request)
        {
            await _scoringService.FinishAsync(request.ScoringId);
        }

        [HttpPost, Authorize(Roles = nameof(RoleType.Admin))]
        [Route("reopen")]
        public async Task ReopenAsync([FromBody] UpdateScoringRequest request)
        {
            await _scoringService.ReopenAsync(request.ScoringId);
        }
    }
}
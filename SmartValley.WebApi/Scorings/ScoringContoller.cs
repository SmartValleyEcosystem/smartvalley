﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Messages.Commands;
using SmartValley.WebApi.Scorings.Requests;
using SmartValley.WebApi.Scorings.Responses;

namespace SmartValley.WebApi.Scorings
{
    [Route("api/scoring")]
    [Authorize]
    public class ScoringContoller : Controller
    {
        private readonly IScoringService _scoringService;
        private readonly IMessageSession _messageSession;

        public ScoringContoller(IScoringService scoringService, IMessageSession messageSession)
        {
            _scoringService = scoringService;
            _messageSession = messageSession;
        }

        [HttpGet]
        public async Task<ScoringResponse> GetByProjectIdAsync(long projectId)
        {
            var scoring = await _scoringService.GetByProjectIdAsync(projectId);
            return ScoringResponse.FromScoring(scoring);
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
    }
}
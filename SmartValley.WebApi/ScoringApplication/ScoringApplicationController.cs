using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
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

        public ScoringApplicationController(IScoringApplicationService scoringApplicationService, IProjectService projectService)
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
        public async Task SaveAsync(long projectId, [FromBody]SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            await _scoringApplicationService.SaveApplicationAsync(projectId, saveScoringApplicationRequest);
        }

        [HttpPost, Authorize]
        [Route("submit")]
        public async Task SubmitAsync(long projectId)
        {
            await _scoringApplicationService.SubmitForScoreAsync(projectId);
        }
    }
}
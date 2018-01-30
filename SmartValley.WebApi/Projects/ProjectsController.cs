using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IcoLab.Common;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.WebApi.Estimates;
using SmartValley.WebApi.Projects.Requests;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.Votings;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IVotingService _votingService;
        private readonly IClock _clock;

        public ProjectsController(IProjectService projectService, IVotingService votingService, IClock clock)
        {
            _projectService = projectService;
            _votingService = votingService;
            _clock = clock;
        }

        [HttpGet]
        public async Task<ProjectDetailsResponse> GetByIdAsync(GetByIdRequest request)
        {
            var details = await _projectService.GetDetailsAsync(request.ProjectId);
            var votingDetails = await _votingService.GetVotingProjectDetailsAsync(request.ProjectId);

            return ProjectDetailsResponse.Create(details, votingDetails, _clock.UtcNow);
        }

        [HttpGet]
        [Route("scored")]
        public async Task<CollectionResponse<ProjectResponse>> GetAllScoredAsync()
        {
            var scoredProjects = await _projectService.GetAllScoredAsync();
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = scoredProjects
                               .Select(ProjectResponse.Create)
                               .OrderByDescending(p => p.Score)
                               .ToArray()
                   };
        }

        [HttpGet]
        [Route("my")]
        public async Task<CollectionResponse<MyProjectsItemResponse>> GetMyProjectsAsync()
        {
            var projectScorings = await _projectService.GetByAuthorAsync(User.Identity.Name);

            var items = new List<MyProjectsItemResponse>();
            foreach (var projectScoring in projectScorings)
                items.Add(await CreateMyProjectsItemResponseAsync(projectScoring));

            return new CollectionResponse<MyProjectsItemResponse> {Items = items};
        }

        [HttpGet]
        [Route("forscoring")]
        public async Task<CollectionResponse<ProjectResponse>> GetForScoringAsync([FromQuery] GetProjectsForScoringRequest request)
        {
            var projects = await _projectService.GetForScoringAsync(request.ExpertiseArea.ToDomain(), User.Identity.Name);
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.Create).ToArray()
                   };
        }

        private async Task<MyProjectsItemResponse> CreateMyProjectsItemResponseAsync(ProjectScoring projectScoring)
        {
            var project = projectScoring.Project;
            var scoring = projectScoring.Scoring;

            if (scoring != null)
                return MyProjectsItemResponse.Create(project, scoring);

            var votingDetails = await _votingService.GetVotingProjectDetailsAsync(project.Id);
            return MyProjectsItemResponse.Create(project, votingDetails, _clock.UtcNow);
        }
    }
}
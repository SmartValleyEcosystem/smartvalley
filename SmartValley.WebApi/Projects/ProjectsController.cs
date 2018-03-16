using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects.Requests;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.Scoring;
using SmartValley.WebApi.Votings;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private const int FileSizeLimitBytes = 5242880;
        private readonly IProjectService _projectService;
        private readonly IVotingService _votingService;
        private readonly IClock _clock;
        private readonly IScoringService _scoringService;

        public ProjectsController(IProjectService projectService, IVotingService votingService, IClock clock, IScoringService scoringService)
        {
            _projectService = projectService;
            _votingService = votingService;
            _scoringService = scoringService;
            _clock = clock;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjectRequest request)
        {
            if (request.TeamMembers == null || 
                !request.TeamMembers.Any() || 
                request.TeamMembers.Any(i=>
                i.Photo == null || 
                i.Photo.Length < 0 || 
                i.Photo.Length > FileSizeLimitBytes))
            {
               // throw new AppErrorException(ErrorCode.InvalidFileUploaded);
            }

            await _projectService.CreateAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<ProjectDetailsResponse> GetByIdAsync(GetByIdRequest request)
        {
            var details = await _projectService.GetDetailsAsync(request.ProjectId);
            var votingDetails = await _votingService.GetVotingProjectDetailsAsync(request.ProjectId);

            return ProjectDetailsResponse.Create(details, votingDetails, _clock.UtcNow);
        }

        [HttpGet("search")]
        public async Task<CollectionResponse<ProjectSearchResponse>> SearchProjectAsync(ProjectSearchRequest request)
        {
            var projects = await _projectService.GetProjectsByNameAsync(request.SearchString);
            return new CollectionResponse<ProjectSearchResponse>
                   {
                       Items = projects.Select(ProjectSearchResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [Route("scored")]
        public async Task<CollectionResponse<ProjectResponse>> GetScoredAsync([FromQuery] GetScoredProjectsRequest request)
        {
            var projects = await _projectService.GetScoredAsync(request.Page, request.PageSize);
            var projectResponses = projects.Select(ProjectResponse.Create).ToArray();

            return new CollectionResponse<ProjectResponse> {Items = projectResponses};
        }

        [HttpGet("scoring"), Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<CollectionResponse<ScoringProjectResponse>> GetScoringProjectsAsync([FromQuery] IReadOnlyCollection<ScoringProjectStatus> statuses)
        {
            var projects = await _scoringService.GetScoringProjectsAsync(statuses);
            var projectResponses = projects.Select(ScoringProjectResponse.Create).ToArray();

            return new CollectionResponse<ScoringProjectResponse> {Items = projectResponses};
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
            var projects = await _projectService.GetForScoringAsync(request.AreaType.ToDomain(), User.GetUserId());
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = projects.Select(ProjectResponse.Create).ToArray()
                   };
        }

        private async Task<MyProjectsItemResponse> CreateMyProjectsItemResponseAsync(ProjectDetails projectDetails)
        {
            var project = projectDetails.Project;
            var scoring = projectDetails.Scoring;

            if (scoring != null)
                return MyProjectsItemResponse.Create(projectDetails);

            var votingDetails = await _votingService.GetVotingProjectDetailsAsync(project.Id);
            return MyProjectsItemResponse.Create(projectDetails, votingDetails, _clock.UtcNow);
        }
    }
}
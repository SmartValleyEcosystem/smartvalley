using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Extensions;
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
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] CreateProjectRequest request)
        {
            await _projectService.CreateAsync(User.GetUserId(), request);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync([FromQuery] long id, [FromBody] UpdateProjectRequest request)
        {
            var isAuthorized = await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId());
            if (!isAuthorized)
                return Unauthorized();

            await _projectService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpPut("{id}/image")]
        [Authorize]
        public async Task<IActionResult> UpdateImageAsync([FromQuery] long id, IFormFile image)
        {
            if (image != null && image.Length > FileSizeLimitBytes)
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            var isAuthorized = await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId());
            if (!isAuthorized)
                return Unauthorized();

            await _projectService.UpdateImageAsync(id, image?.ToAzureFile());
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var isAuthorized = await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId());
            if (!isAuthorized)
                return Unauthorized();

            await _projectService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("teammember")]
        [Authorize]
        public async Task<IActionResult> UploadTeamMemberPhotoAsync([FromForm] AddProjectTeamMemberPhotoRequest request, IFormFile photo)
        {
            if (photo == null ||
                photo.Length < 0 ||
                photo.Length > FileSizeLimitBytes)
            {
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);
            }

            if (!await _projectService.IsAuthorizedToEditProjectTeamMemberAsync(User.GetUserId(), request.ProjectTeamMemberId))
                return Unauthorized();

            await _projectService.UpdateTeamMemberPhotoAsync(request.ProjectTeamMemberId, photo.ToAzureFile());
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ProjectSummaryResponse> GetSummaryAsync([FromQuery] long id)
        {
            var project = await _projectService.GetAsync(id);
            var scoring = await _scoringService.GetByProjectIdAsync(id);

            return ProjectSummaryResponse.Create(project, scoring);
        }

        [HttpGet("{id}/about")]
        public async Task<ProjectAboutResponse> GetAboutAsync([FromQuery] long id)
        {
            var project = await _projectService.GetAsync(id);
            var teamMembers = await _projectService.GetTeamAsync(id);

            return ProjectAboutResponse.Create(project, teamMembers);
        }

        [HttpGet("{id}/details")]
        public async Task<ProjectDetailsResponse> GetDetailsAsync([FromQuery] long id)
        {
            var details = await _projectService.GetDetailsAsync(id);
            var votingDetails = await _votingService.GetVotingProjectDetailsAsync(id);

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
        public async Task<PartialCollectionResponse<ProjectResponse>> GetScoredAsync([FromQuery] GetScoredProjectsRequest request)
        {
            var projectsQuery = new SearchProjectsQuery(
                request.Offset,
                request.Count,
                request.SearchString,
                request.StageType,
                request.CountryCode,
                request.CategoryType,
                request.MinimumScore,
                request.MaximumScore,
                request.OrderBy,
                request.Direction
            );
            var projects = await _projectService.GetScoredAsync(projectsQuery);

            var totalCount = await _projectService.GetScoredTotalCountAsync(projectsQuery);

            var projectResponses = projects.Select(ProjectResponse.Create).ToArray();

            return new PartialCollectionResponse<ProjectResponse>(request.Offset, projectResponses.Length, totalCount, projectResponses);
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
            var projectScorings = await _projectService.GetByAuthorIdAsync(User.GetUserId());

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
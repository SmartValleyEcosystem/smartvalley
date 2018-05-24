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
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects.Requests;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.Scorings;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IScoringService _scoringService;
        private readonly IScoringApplicationRepository _scoringApplicationRepository;

        public ProjectsController(
            IProjectService projectService,
            IScoringService scoringService,
            IScoringApplicationRepository scoringApplicationRepository)
        {
            _projectService = projectService;
            _scoringService = scoringService;
            _scoringApplicationRepository = scoringApplicationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] QueryProjectsRequest request)
        {
            if (request.IsPrivate == true && !User.IsInRole(nameof(RoleType.Admin)))
            {
                return Unauthorized();
            }

            var projectsQuery = new ProjectsQuery(
                request.Offset,
                request.Count,
                request.IsPrivate,
                request.OnlyScored,
                request.SearchString,
                request.Stage,
                request.CountryCode,
                request.Category,
                request.MinimumScore,
                request.MaximumScore,
                request.OrderBy,
                request.SortDirection,
                request.ScoringStatuses
            );

            var projects = await _projectService.GetAsync(projectsQuery);

            return Ok(projects.ToPartialCollectionResponse(ProjectResponse.Create));
        }

        [HttpPost]
        [Authorize]
        public async Task<ProjectAboutResponse> PostAsync([FromBody] CreateProjectRequest request)
        {
            var project = await _projectService.CreateAsync(User.GetUserId(), request);

            return ProjectAboutResponse.Create(project);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ProjectAboutResponse> PutAsync(long id, [FromBody] UpdateProjectRequest request)
        {
            if (!await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId()))
                throw new AppErrorException(ErrorCode.UserNotAuthor);

            var project = await _projectService.UpdateAsync(id, request);

            return ProjectAboutResponse.Create(project);
        }

        [HttpPut("{id}/image")]
        [Authorize]
        public async Task<IActionResult> UpdateImageAsync(long id, IFormFile image)
        {
            if (!image.IsImageValid())
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            if (!await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId()))
                return Unauthorized();

            await _projectService.UpdateImageAsync(id, image?.ToAzureFile());
            return NoContent();
        }

        [HttpDelete("{id}/image")]
        [Authorize]
        public async Task<IActionResult> DeleteImageAsync(long id)
        {
            if (!await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId()))
                return Unauthorized();

            await _projectService.DeleteProjectImageAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            if (!await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId()))
                return Unauthorized();

            await _projectService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("teammembers")]
        [Authorize]
        public async Task<IActionResult> UploadTeamMemberPhotoAsync([FromForm] AddProjectTeamMemberPhotoRequest request, IFormFile photo)
        {
            if (!photo.IsImageValid())
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            if (!await _projectService.IsAuthorizedToEditProjectTeamMemberAsync(User.GetUserId(), request.ProjectId))
                return Unauthorized();

            await _projectService.UpdateTeamMemberPhotoAsync(request.ProjectId, request.ProjectTeamMemberId, photo.ToAzureFile());
            return NoContent();
        }

        [HttpDelete("{projectId}/teammembers/{teamMemberId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTeamMemberPhotoAsync(long projectId, long teamMemberId)
        {
            if (!await _projectService.IsAuthorizedToEditProjectTeamMemberAsync(User.GetUserId(), projectId))
                return Unauthorized();

            await _projectService.DeleteTeamMemberPhotoAsync(projectId, teamMemberId);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ProjectSummaryResponse> GetSummaryAsync(long id)
        {
            var project = await _projectService.GetByIdAsync(id);
            var scoring = await _scoringService.GetByProjectIdAsync(id);
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(id);

            return ProjectSummaryResponse.Create(project, scoring, scoringApplication);
        }

        [HttpGet("{id}/about")]
        public async Task<ProjectAboutResponse> GetAboutAsync(long id)
        {
            var project = await _projectService.GetByIdAsync(id);

            return ProjectAboutResponse.Create(project);
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

        [HttpGet("scoring"), Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<CollectionResponse<ScoringProjectResponse>> GetScoringProjectsAsync([FromQuery] IReadOnlyCollection<ScoringProjectStatus> statuses)
        {
            var projects = await _scoringService.GetScoringProjectsAsync(statuses);
            var projectResponses = projects.Select(ScoringProjectResponse.Create).ToArray();

            return new CollectionResponse<ScoringProjectResponse> {Items = projectResponses};
        }

        [HttpGet]
        [Authorize]
        [Route("my")]
        public async Task<MyProjectResponse> GetMyProjectAsync()
        {
            var project = await _projectService.GetByAuthorIdAsync(User.GetUserId());
            if (project == null)
                return null;

            return new MyProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Category = (int) project.Category,
                       Description = project.Description,
                       Stage = (int) project.Stage,
                       CountryCode = project.Country.Code,
                       TeamMembers = project.TeamMembers?.Select(ProjectTeamMemberResponse.Create).ToArray(),
                       IcoDate = project.IcoDate,
                       Website = project.Website,
                       ContactEmail = project.ContactEmail,
                       WhitePaperLink = project.WhitePaperLink,
                       Facebook = project.Facebook,
                       Reddit = project.Reddit,
                       BitcoinTalk = project.BitcoinTalk,
                       Telegram = project.Telegram,
                       Github = project.Github,
                       Medium = project.Medium,
                       Twitter = project.Twitter,
                       Linkedin = project.Linkedin,
                       ImageUrl = project.ImageUrl
                   };
        }
    }
}
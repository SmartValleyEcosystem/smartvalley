using System.Collections.Generic;
using System.IO;
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
using SmartValley.WebApi.Users;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IScoringService _scoringService;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserService _userService;
        private readonly IScoringApplicationRepository _scoringApplicationRepository;

        public ProjectsController(
            IProjectService projectService,
            IScoringService scoringService,
            ICountryRepository countryRepository,
            IScoringApplicationRepository scoringApplicationRepository,
            IUserService userService)
        {
            _projectService = projectService;
            _scoringService = scoringService;
            _countryRepository = countryRepository;
            _userService = userService;
            _scoringApplicationRepository = scoringApplicationRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ProjectAboutResponse> PostAsync([FromBody] CreateProjectRequest request)
        {
            var project = await _projectService.CreateAsync(User.GetUserId(), request);
            var teamMembers = await _projectService.GetTeamAsync(project.Id);

            return ProjectAboutResponse.Create(project, teamMembers);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ProjectAboutResponse> PutAsync(long id, [FromBody] UpdateProjectRequest request)
        {
            if (!await _projectService.IsAuthorizedToEditProjectAsync(id, User.GetUserId()))
                throw new AppErrorException(ErrorCode.UserNotAuthor);

            var project = await _projectService.UpdateAsync(id, request);
            var teamMembers = await _projectService.GetTeamAsync(project.Id);

            return ProjectAboutResponse.Create(project, teamMembers);
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

        [HttpPut("teammember")]
        [Authorize]
        public async Task<IActionResult> UploadTeamMemberPhotoAsync([FromForm] AddProjectTeamMemberPhotoRequest request, IFormFile photo)
        {
            if (!photo.IsImageValid())
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            if (!await _projectService.IsAuthorizedToEditProjectTeamMemberAsync(User.GetUserId(), request.ProjectTeamMemberId))
                return Unauthorized();

            await _projectService.UpdateTeamMemberPhotoAsync(request.ProjectTeamMemberId, photo.ToAzureFile());
            return NoContent();
        }

        [HttpDelete("teammember/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTeamMemberPhotoAsync(long id)
        {
            if (!await _projectService.IsAuthorizedToEditProjectTeamMemberAsync(User.GetUserId(), id))
                return Unauthorized();

            await _projectService.DeleteTeamMemberPhotoAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ProjectSummaryResponse> GetSummaryAsync(long id)
        {
            var project = await _projectService.GetAsync(id);
            var scoring = await _scoringService.GetByProjectIdAsync(id);
            var country = await _countryRepository.GetByIdAsync(project.CountryId);
            var author = await _userService.GetByIdAsync(project.AuthorId);
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(id);

            return ProjectSummaryResponse.Create(project, country, scoring, scoringApplication, author);
        }

        [HttpGet("{id}/about")]
        public async Task<ProjectAboutResponse> GetAboutAsync(long id)
        {
            var project = await _projectService.GetAsync(id);
            var teamMembers = await _projectService.GetTeamAsync(id);

            return ProjectAboutResponse.Create(project, teamMembers);
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

        [HttpGet("query")]
        public async Task<PartialCollectionResponse<ProjectResponse>> QueryAsync([FromQuery] QueryProjectsRequest request)
        {
            var projectsQuery = new ProjectsQuery(
                request.Offset,
                request.Count,
                request.OnlyScored,
                request.SearchString,
                request.Stage,
                request.CountryCode,
                request.Category,
                request.MinimumScore,
                request.MaximumScore,
                request.OrderBy,
                request.SortDirection
            );
            var projects = await _projectService.QueryAsync(projectsQuery);

            var totalCount = await _projectService.GetQueryTotalCountAsync(projectsQuery);

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
        [Authorize]
        [Route("my")]
        public async Task<MyProjectResponse> GetMyProjectAsync()
        {
            var project = await _projectService.GetByAuthorIdAsync(User.GetUserId());
            if (project == null)
                return null;

            project.TeamMembers = await _projectService.GetTeamAsync(project.Project.Id);

            return new MyProjectResponse
                   {
                       Id = project.Project.Id,
                       Name = project.Project.Name,
                       Category = (int) project.Project.Category,
                       Description = project.Project.Description,
                       Stage = (int) project.Project.Stage,
                       CountryCode = project.Country.Code,
                       TeamMembers = project.TeamMembers?.Select(ProjectTeamMemberResponse.Create).ToArray(),
                       IcoDate = project.Project.IcoDate,
                       Website = project.Project.Website,
                       ContactEmail = project.Project.ContactEmail,
                       WhitePaperLink = project.Project.WhitePaperLink,
                       Facebook = project.Project.Facebook,
                       Reddit = project.Project.Reddit,
                       BitcoinTalk = project.Project.BitcoinTalk,
                       Telegram = project.Project.Telegram,
                       Github = project.Project.Github,
                       Medium = project.Project.Medium,
                       Twitter = project.Project.Twitter,
                       Linkedin = project.Project.Linkedin,
                       ImageUrl = project.Project.ImageUrl
                   };
        }
    }
}
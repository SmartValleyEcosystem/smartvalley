using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Projects.Requests;
using SmartValley.WebApi.Projects.Responses;
using SmartValley.WebApi.Votings;
using SmartValley.WebApi.WebApi;
using AreaType = SmartValley.Domain.Entities.AreaType;

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

        [HttpGet("scoring"), Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<CollectionResponse<ScoringProjectResponse>> GetScoringProjectsAsync([FromQuery] string queryStatuses)
        {
            var statuses = queryStatuses?.Split(',').Select(i => (ScoringProjectStatus) int.Parse(i));

            var experts = new List<AreaExpertResponse>
            {
                new AreaExpertResponse{Addresses = new List<string>{ "asdasdas" , "asdasdas" }, Area = new Area{Id = AreaType.Analyst, Name = "Аналитик"}},
                new AreaExpertResponse{Addresses = new List<string>{ "asdasdas" }, Area = new Area{Id = AreaType.Lawyer, Name = "Юрист"}},
                new AreaExpertResponse{Addresses = new List<string>{ "asdasdas" , "asdasdas" , "asdasdas" }, Area = new Area{Id = AreaType.Tech, Name = "Айтишник"}},
                new AreaExpertResponse{Addresses = new List<string>{ "asdasdas" , "asdasdas" , "asdasdas" }, Area = new Area{Id = AreaType.Hr, Name = "HR"}}

            };
            var items = new List<ScoringProjectResponse>()
                        {
                new ScoringProjectResponse(){Address = "123", Name = "dfsd", ProjectId = "asdas", StartDate = DateTime.Now, EndDate = DateTime.Now + TimeSpan.FromDays(5), Status = ScoringProjectStatus.InProgress, AreasExperts = experts},
                new ScoringProjectResponse(){Address = "123", Name = "zxczx", ProjectId = "dfdf", StartDate = DateTime.Now + TimeSpan.FromDays(1), EndDate = DateTime.Now + TimeSpan.FromDays(5),  Status = ScoringProjectStatus.Rejected, AreasExperts = experts},
                new ScoringProjectResponse(){Address = "123", Name = "qweqwe", ProjectId = "zxczx", StartDate = DateTime.Now + TimeSpan.FromDays(3), EndDate = DateTime.Now + TimeSpan.FromDays(5),  Status = ScoringProjectStatus.AcceptedAndDoNotEstimate, AreasExperts = experts}
                        };

            if (queryStatuses == null || statuses.Any(i => i == ScoringProjectStatus.All))
            {
                return new CollectionResponse<ScoringProjectResponse> { Items = items };
            }
            return new CollectionResponse<ScoringProjectResponse> { Items = items.Where(i => statuses.Contains(i.Status)).ToArray() };
        }

        [HttpGet]
        [Route("my")]
        public async Task<CollectionResponse<MyProjectsItemResponse>> GetMyProjectsAsync()
        {
            var projectScorings = await _projectService.GetByAuthorAsync(User.Identity.Name);

            var items = new List<MyProjectsItemResponse>();
            foreach (var projectScoring in projectScorings)
                items.Add(await CreateMyProjectsItemResponseAsync(projectScoring));

            return new CollectionResponse<MyProjectsItemResponse> { Items = items };
        }

        [HttpGet]
        [Route("forscoring")]
        public async Task<CollectionResponse<ProjectResponse>> GetForScoringAsync([FromQuery] GetProjectsForScoringRequest request)
        {
            var projects = await _projectService.GetForScoringAsync(request.AreaType.ToDomain(), User.Identity.Name);
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
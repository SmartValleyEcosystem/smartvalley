using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Votings.Responses;
using SmartValley.WebApi.WebApi;
using System.Linq;
using SmartValley.Domain;

namespace SmartValley.WebApi.Votings
{
    [Route("api/votings")]
    public class VotingsController : Controller
    {
        private readonly IVotingService _votingService;
        private readonly IProjectService _projectService;
        private readonly IClock _clock;

        public VotingsController(IVotingService votingService, IProjectService projectService, IClock clock)
        {
            _votingService = votingService;
            _projectService = projectService;
            _clock = clock;
        }

        [HttpGet]
        [Route("current")]
        public async Task<GetSprintResponse> GetCurrentSprintAsync()
        {
            var sprint = await _votingService.GetLastSprintDetailsAsync();
            if (sprint == null || _clock.UtcNow > sprint.EndDate)
                return new GetSprintResponse();
            return await GetSprintResponseAsync(sprint);
        }

        [HttpGet]
        [Route("completed")]
        public async Task<CollectionResponse<SprintResponse>> GetCompletedSprintsAsync()
        {
            var sprints = await _votingService.GetCompletedSprintsAsync();
            return new CollectionResponse<SprintResponse>
                   {
                       Items = sprints.Select(SprintResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [Route("{address}")]
        public async Task<GetSprintResponse> GetSprintDetailsByAddressAsync(string address)
        {
            var sprint = await _votingService.GetSprintDetailsByAddressAsync(address);
            if (sprint == null)
                return new GetSprintResponse();
            return await GetSprintResponseAsync(sprint);
        }

        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> StartSprintAsync()
        {
            await _votingService.StartSprintAsync();
            return NoContent();
        }

        private async Task<GetSprintResponse> GetSprintResponseAsync(VotingSprintDetails sprint)
        {
            var investorVotes = await _votingService.GetVotesAsync(sprint.Address, User.Identity.Name);
            var projects = await _projectService.GetByExternalIdsAsync(sprint.ProjectsExternalIds);

            return new GetSprintResponse {Sprint = VotingSprintResponse.Create(sprint, projects, investorVotes, _clock.UtcNow)};
        }
    }
}
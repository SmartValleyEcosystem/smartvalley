using System.Threading.Tasks;
using IcoLab.Common;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Votings.Responses;
using SmartValley.WebApi.WebApi;
using System.Linq;

namespace SmartValley.WebApi.Votings
{
    [Route("api/votings")]
    public class VotingsController : Controller
    {
        private readonly IVotingService _votingService;
        private readonly IProjectService _projectService;
        private readonly IDateTime _dateTime;

        public VotingsController(IVotingService votingService, IProjectService projectService, IDateTime dateTime)
        {
            _votingService = votingService;
            _projectService = projectService;
            _dateTime = dateTime;
        }

        [HttpGet]
        [Route("current")]
        public async Task<GetCurrentSprintResponse> GetCurrentSprintAsync()
        {
            var lastSprint = await _votingService.GetLastSprintDetailsAsync();
            if (lastSprint == null || _dateTime.UtcNow > lastSprint.EndDate)
                return new GetCurrentSprintResponse();

            var investorVotes = await _votingService.GetVotesAsync(lastSprint.Address, User.Identity.Name);
            var projects = await _projectService.GetByExternalIdsAsync(lastSprint.ProjectsExternalIds);

            return new GetCurrentSprintResponse { LastSprint = VotingSprintResponse.Create(lastSprint, projects, investorVotes) };
        }

        [HttpGet]
        [Route("finished")]
        public async Task<CollectionResponse<SprintResponse>> GetFinishedSprintsAsync()
        {
            var sprints = await _votingService.GetFinishedSprintsAsync();
            return new CollectionResponse<SprintResponse>
            {
                Items = sprints.Select(SprintResponse.Create).ToArray()
            };
        }

        [HttpGet]
        [Route("{address}")]
        public async Task<IActionResult> GetSprintDetailsByAddressAsync(string address)
        {
            var sprint = await _votingService.GetSprintDetailsByAddressAsync(address);
            if (sprint == null)
                return NotFound();

            var investorVotes = await _votingService.GetVotesAsync(sprint.Address, User.Identity.Name);
            var projects = await _projectService.GetByExternalIdsAsync(sprint.ProjectsExternalIds);
            return Ok(VotingSprintResponse.Create(sprint, projects, investorVotes));
        }

        [HttpGet]
        [Route("start")]
        public async Task<IActionResult> StartSprint()
        {
            await _votingService.StartSprintAsync();
            return NoContent();
        }
    }
}
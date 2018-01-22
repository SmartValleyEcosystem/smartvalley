using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Votings.Responses;

namespace SmartValley.WebApi.Votings
{
    [Route("api/votings")]
    public class VotingsController : Controller
    {
        private readonly IVotingService _votingService;
        private readonly IProjectService _projectService;

        public VotingsController(IVotingService votingService, IProjectService projectService)
        {
            _votingService = votingService;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("last")]
        public async Task<GetLastSprintResponse> GetLastSprintAsync()
        {
            var lastSprint = await _votingService.GetLastSprintDetailsAsync();
            if (lastSprint == null)
                return new GetLastSprintResponse();

            var investorVotes = await _votingService.GetVotesAsync(lastSprint.Address, User.Identity.Name);
            var projects = await _projectService.GetByExternalIdsAsync(lastSprint.ProjectExternalIds);

            return new GetLastSprintResponse {LastSprint = VotingSprintResponse.Create(lastSprint, projects, investorVotes) };
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
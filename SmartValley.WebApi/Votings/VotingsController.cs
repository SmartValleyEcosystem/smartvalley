using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Votings.Responses;

namespace SmartValley.WebApi.Votings
{
    [Route("api/votings")]
    public class VotingsController : Controller
    {
        private readonly IVotingService _votingService;

        public VotingsController(IVotingService votingService)
        {
            _votingService = votingService;
        }

        [HttpGet]
        [Route("last")]
        public async Task<GetLastSprintResponse> GetLastSprintAsync()
        {
            var lastSprint = await _votingService.GetLastSprintDetailsAsync();
            return new GetLastSprintResponse
                   {
                       DoesExist = lastSprint != null,
                       EndDate = lastSprint?.EndDate,
                       MaximumScore = lastSprint?.MaximumScore,
                       StartDate = lastSprint?.StartDate
                   };
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
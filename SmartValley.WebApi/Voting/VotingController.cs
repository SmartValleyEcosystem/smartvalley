using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Voting.Responses;

namespace SmartValley.WebApi.Voting
{
    [Route("api/sprints")]
    public class VotingController : Controller
    {
        private readonly IVotingService _votingService;

        public VotingController(IVotingService votingService)
        {
            _votingService = votingService;
        }

        [HttpGet]
        [Route("last")]
        public async Task<GetLastSprintResponse> GetLastSprintAsync()
        {
            var lastSprint = await _votingService.GetLastSprintAsync();
            return new GetLastSprintResponse { LastSprint = lastSprint };
        }
    }
}

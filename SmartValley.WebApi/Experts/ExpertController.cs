using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Experts.Responses;

namespace SmartValley.WebApi.Experts
{
    [Route("api/experts")]
    public class ExpertController : Controller
    {
        private readonly IExpertService _expertService;

        public ExpertController(IExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpGet]
        [Route("{address}/status")]
        public async Task<GetExpertStatusResponse> GetExpertStatusAsync(string address)
        {
            var isApplied = await _expertService.IsAppliedAsync(address);
            var isConfirmed = await _expertService.IsConfirmedAsync(address);
            return new GetExpertStatusResponse { IsConfirmed = isConfirmed, IsApplied = isApplied };
        }

        [HttpPost]
        [Route("apply")]
        public async Task<IActionResult> ApplyAsync([FromBody] ExpertApplicationRequest request)
        {
            await _expertService.CreateApplicationAsync(request);
            return NoContent();
        }
    }
}

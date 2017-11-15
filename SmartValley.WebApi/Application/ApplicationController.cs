using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartValley.WebApi.Application
{
    [Produces("application/json")]
    [Route("api/application")]
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ApplicationRequest model)
        {
            await _service.CreateApplication(model);
            return Ok(model);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartValley.WebApi.Logging.Requests;

namespace SmartValley.WebApi.Logging
{
    [Route("api/logging")]
    public class LoggingController : Controller
    {
        private readonly ILogger _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpPost("error")]
        public IActionResult Post([FromBody] LogErrorRequest request)
        {
            _logger.LogError($"User: '{User.Identity.Name}', {request}");
            return NoContent();
        }
    }
}

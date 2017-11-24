using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartValley.WebApi.Applications
{
    [Route("api/applications")]
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _service;

        public ApplicationsController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<ApplicationResponse> GetByProjectIdAsync(GetByProjectIdRequest request)
        {
            return _service.GetByProjectIdAsync(request.ProjectId);
        }

        [HttpPost]
        public Task Post([FromBody] ApplicationRequest request) => _service.CreateAsync(request);
    }
}
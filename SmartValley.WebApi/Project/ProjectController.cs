using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Project.Requests;

namespace SmartValley.WebApi.Project
{
    [Route("/api/project")]
    [Authorize(AuthenticationSchemes = EcdsaAuthenticationOptions.DefaultScheme)]
    public class ProjectController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody] CreateProjectRequest request)
        {
            return Ok();
        }
    }
}
﻿using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicationRequest request)
        {
            await _service.CreateApplicationAsync(request);
            return Ok(request);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Applications.Requests;
using SmartValley.WebApi.Applications.Responses;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Applications
{
    [Route("api/applications")]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _service;

        public ApplicationsController(IApplicationService service)
        {
            _service = service;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] ApplicationRequest request)
        {
            await _service.CreateAsync(User.GetUserId(), request);
            return NoContent();
        }

        [HttpGet("countries")]
        public async Task<CollectionResponse<CountryResponse>> GetCountriesAsync()
        {
            var countries = await _service.GetCountriesAsync();
            return new CollectionResponse<CountryResponse>
                   {
                       Items = countries.Select(c => new CountryResponse
                                                     {
                                                         Code = c.Code
                                                     }).ToArray()
                   };
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Admin.Request;
using SmartValley.WebApi.Admin.Response;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Admin
{
    [Route("api/admin")]
    //[Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminRequest request)
        {
            await _service.AddAsync(request.address);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _service.GetAllAsync();
            return Ok(new CollectionResponse<AdminResponse>
            {
                Items = admins.Select(i => new AdminResponse { Address = i }).ToArray()
            });
        }

        [HttpDelete]
        [Route("{address}")]
        public async Task<IActionResult> Delete(AdminRequest request)
        {
            await _service.DeleteAsync(request.address);
            return NoContent();
        }

        [HttpPost]
        [Route("isAdmin")]
        public async Task<IActionResult> IsAdmin([FromBody] AdminRequest request)
        {
            return Ok(new IsAdminResponse { IsAdmin = await _service.IsAdminAsync(request.address) });
        }
    }
}

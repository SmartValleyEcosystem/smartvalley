using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
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
        private readonly EthereumClient _ethereumClient;

        public AdminController(IAdminService service, EthereumClient ethereumClient)
        {
            _service = service;
            _ethereumClient = ethereumClient;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _service.AddAsync(request.Address);
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
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _service.DeleteAsync(request.Address);
            return NoContent();
        }

        [HttpPost]
        [Route("isAdmin")]
        public async Task<IActionResult> IsAdmin([FromBody] AdminRequest request)
        {
            return Ok(new IsAdminResponse { IsAdmin = await _service.IsAdminAsync(request.Address) });
        }
    }
}

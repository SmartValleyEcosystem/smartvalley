using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Admin.Request;
using SmartValley.WebApi.Admin.Response;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Admin
{
    [Route("api/admin")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;
        private readonly IExpertService _expertService;
        private readonly EthereumClient _ethereumClient;
        private readonly IAuthenticationService _authenticationService;

        public AdminController(
            IExpertService expertService, 
            IAdminService service, 
            EthereumClient ethereumClient, 
            IAuthenticationService authenticationService)
        {
            _service = service;
            _ethereumClient = ethereumClient;
            _expertService = expertService;
            _authenticationService = authenticationService;
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
                Items = admins.Select(i => new AdminResponse { Address = i.Address, Email = i.Email }).ToArray()
            });
        }

        [HttpPut("experts")]
        public async Task<IActionResult> UpdateExpertAsync([FromBody] AdminExpertUpdateRequest request)
        {
            await _expertService.UpdateAsync(request.Address, request);
            await _authenticationService.ChangeEmailAsync(request.Address, request.Email);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string address, string transactionHash)
        {
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);
            await _service.DeleteAsync(address);
            return NoContent();
        }
    }
}

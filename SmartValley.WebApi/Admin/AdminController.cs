using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.Ethereum;
using SmartValley.WebApi.Admin.Request;
using SmartValley.WebApi.Admin.Response;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Users;
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
        private readonly IUserService _userService;

        public AdminController(
            IExpertService expertService,
            IAdminService service,
            EthereumClient ethereumClient,
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _service = service;
            _ethereumClient = ethereumClient;
            _expertService = expertService;
            _authenticationService = authenticationService;
            _userService = userService;
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
                          Items = admins.Select(i => new AdminResponse {Address = i.Address, Email = i.Email}).ToArray()
                      });
        }

        [HttpGet("users")]
        public async Task<PartialCollectionResponse<UserResponse>> GetAllUsers(CollectionPageRequest request)
        {
            var totalCount = await _userService.GetTotalCountAsync();
            var users = await _userService.GetAllAsync(request.Offset, request.Count);

            return new PartialCollectionResponse<UserResponse>(
                request.Offset, users.Count, totalCount, users.Select(UserResponse.Create).ToArray());
        }

        [HttpPut("users")]
        public async Task<IActionResult> PutUserAsync([FromBody] AdminUserUpdateRequest request)
        {
            var user = await _userService.GetByAddressAsync(request.Address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            await _userService.SetCanCreatePrivateProjectsAsync(user.Address, request.CanCreatePrivateProjects);

            return NoContent();
        }

        [HttpPut("experts/availability")]
        public async Task<IActionResult> SetExpertAvailabilityAsync([FromBody] AdminSetAvailabilityRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.SetAvailabilityAsync(request.Address, request.Value);
            return NoContent();
        }

        [HttpPut("experts/areas")]
        public async Task<IActionResult> UpdateExpertAreasAsync([FromBody] AdminExpertUpdateAreasRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.UpdateAreasAsync(request.Address, request.Areas);
            return NoContent();
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
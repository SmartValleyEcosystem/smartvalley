using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Users.Responses;

namespace SmartValley.WebApi.Users
{
    [Route("api/users")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{address}")]
        public async Task<GetUserResponse> GetByAddressAsync(string address)
        {
            var user = await _userService.GetByAddressAsync(address);
            return new GetUserResponse { Address = user.Address, Email = user.Email, IsEmailConfirmed = user.IsEmailConfirmed };
        }
    }
}

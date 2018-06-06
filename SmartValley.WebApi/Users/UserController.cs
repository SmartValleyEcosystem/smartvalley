using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Authentication.Requests;
using SmartValley.WebApi.Authentication.Responses;
using SmartValley.WebApi.Users.Requests;
using SmartValley.WebApi.Users.Responses;

namespace SmartValley.WebApi.Users
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("{address}")]
        public async Task<UserResponse> GetByAddressAsync(string address)
        {
            var user = await _userService.GetByAddressAsync(address);
            return UserResponse.Create(user);
        }

        [HttpGet("{address}/email")]
        public async Task<EmailResponse> GetEmailBySignatureAsync(EmailRequest request)
        {
            var email = await _authenticationService.GetEmailBySignatureAsync(request.Address, request.Signature, request.SignedText);

            return new EmailResponse
                   {
                       Email = email
                   };
        }

        [Authorize]
        [HttpPut]
        public async Task<EmptyResponse> UpdateUserAsync([FromBody] UpdateUserRequest request)
        {
            await _userService.UpdateAsync(User.Identity.Name, request);
            return new EmptyResponse();
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
        {
            await _authenticationService.ChangeEmailAsync(User.Identity.Name, request.Email);
            return Ok(new EmptyResponse());
        }
    }
}
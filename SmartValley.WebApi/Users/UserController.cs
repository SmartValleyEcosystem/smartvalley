using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Authentication.Requests;
using SmartValley.WebApi.Authentication.Responses;
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
        public async Task<GetUserResponse> GetByAddressAsync(string address)
        {
            var user = await _userService.GetByAddressAsync(address);
            return new GetUserResponse
                   {
                       Address = user?.Address,
                       Email = user?.Email,
                       Name = user?.Name,
                       About = user?.About,
                       IsEmailConfirmed = user != null && user.IsEmailConfirmed
                   };
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
        [HttpPut("{address}")]
        public async Task<EmptyResponse> UpdateUserAsync(string address, [FromBody] UpdateUserRequest request)
        {
            await _userService.UpdateAsync(address, request.Name, request.About);
            return new EmptyResponse();
        }

        [Authorize]
        [HttpPut("{address}/email")]
        public async Task<IActionResult> ChangeEmailAsync(string address, [FromBody] ChangeEmailRequest request)
        {
            if (!address.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            await _authenticationService.ChangeEmailAsync(address, request.Email);
            return Ok(new EmptyResponse());
        }
    }
}
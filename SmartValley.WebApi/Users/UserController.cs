using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Entities;
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
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<GetUserResponse> GetByAddressAsync(string address)
        {
            var user = await _userService.GetByAddressAsync(address);
            return new GetUserResponse
                   {
                       Address = user?.Address,
                       Email = user?.Email,
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
        public async Task<EmptyResponse> UpdateUserAsync([FromBody] UpdateUserRequest request)
        {
            await _userService.UpdateAsync(User.Identity.Name, request.Name, request.About);
            return new EmptyResponse();
        }

        [Authorize]
        [HttpPut("{address}/email")]
        public async Task<EmptyResponse> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
        {
            await _authenticationService.ChangeEmailAsync(User.Identity.Name, request.Email);
            return new EmptyResponse();
        }
    }
}
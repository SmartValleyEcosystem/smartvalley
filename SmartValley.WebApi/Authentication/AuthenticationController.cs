using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Authentication.Requests;
using SmartValley.WebApi.Authentication.Responses;

namespace SmartValley.WebApi.Authentication
{
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<AuthenticationResponse> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            var identity = await _authenticationService.AuthenticateAsync(request);

            return new AuthenticationResponse
                   {
                       Token = identity.Token,
                       Roles = identity.Roles
                   };
        }

        [HttpPost("register")]
        public async Task<EmptyResponse> RegisterAsync([FromBody] RegistrationRequest request)
        {
            await _authenticationService.RegisterAsync(request);
            return new EmptyResponse();
        }

        [HttpPut("confirm")]
        public async Task<EmptyResponse> ConfrimEmailAsync([FromBody] ConfirmEmailRequest request)
        {
            await _authenticationService.ConfirmEmailAsync(request.Address, request.Token, request.Email);
            return new EmptyResponse();
        }

        [HttpPost("resend")]
        public async Task<EmptyResponse> ResendEmailAsync([FromBody] ReSendEmailRequest request)
        {
            await _authenticationService.ResendEmailAsync(request.Address);
            return new EmptyResponse();
        }
    }
}
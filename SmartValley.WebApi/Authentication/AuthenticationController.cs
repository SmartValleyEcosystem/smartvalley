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

        public AuthenticationController(AuthenticationService authenticationService)
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
        public Task RegisterAsync([FromBody] RegistrationRequest request)
        {
            return _authenticationService.RegisterAsync(request);
        }
    }
}
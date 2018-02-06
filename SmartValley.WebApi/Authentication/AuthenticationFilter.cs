using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Authentication
{
    public class AuthenticationFilter : IAsyncActionFilter
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationFilter(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            await next();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
                return;

            if (_authenticationService.ShouldRefreshToken(token))
            {
                var identity = await _authenticationService.RefreshAccessTokenAsync(context.HttpContext.User.Identity.Name);

                context.HttpContext.Response.Headers.Add(Headers.XNewAuthToken, identity.Token);
                context.HttpContext.Response.Headers.Add(Headers.XNewRoles, string.Join(",", identity.Roles));
            }
        }
    }
}
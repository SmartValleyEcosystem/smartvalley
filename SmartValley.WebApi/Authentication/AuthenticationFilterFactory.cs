using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace SmartValley.WebApi.Authentication
{
    public class AuthenticationFilterFactory : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var authenticationService = serviceProvider.GetService<IAuthenticationService>();
            return new AuthenticationFilter(authenticationService);
        }
    }
}
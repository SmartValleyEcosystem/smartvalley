using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Authentication
{
    public class MetamaskAuthenticationHandler : AuthenticationHandler<MetamaskAuthenticationOptions>
    {
        public MetamaskAuthenticationHandler(IOptionsMonitor<MetamaskAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {         
            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XNewEthereumAddress, out var ethAddress))
            {
                return (AuthenticateResult.Fail("Cannot read eth address header."));
            }

            if (!Request.Headers.TryGetValue(SvCustomCorsConstants.XNewEthereumAddress, out var signature))
            {
               return AuthenticateResult.Fail("Cannot read signature header.");
            }

            //TODO Check address and sign here

            var identities = new List<ClaimsIdentity> {new ClaimsIdentity(MetamaskAuthenticationOptions.DefaultScheme) };
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), MetamaskAuthenticationOptions.DefaultScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
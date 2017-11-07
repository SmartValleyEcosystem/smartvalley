using Microsoft.AspNetCore.Authentication;

namespace SmartValley.WebApi.Authentication
{
    public class MetamaskAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Metamask";
    }
}
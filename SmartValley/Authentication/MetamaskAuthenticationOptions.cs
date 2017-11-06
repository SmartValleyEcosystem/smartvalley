using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SmartValley.Authentication
{
    public class MetamaskAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Metamask";
    }
}
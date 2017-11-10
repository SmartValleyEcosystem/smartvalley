using Microsoft.AspNetCore.Authentication;

namespace SmartValley.WebApi.Authentication
{
    public class EcdsaAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Metamask";
    }
}
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartValley.WebApi.Authentication
{
    public class AuthenticationOptions
    {
        public string BaseUrl { get; set; }

        public const string Issuer = "SmartValley";
        public const string Audience = "https://smartvalley.io/";
        public const int LifetimeInMinutes = 10;

        const string Key = "64QAJL2ddCWPwZ29";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
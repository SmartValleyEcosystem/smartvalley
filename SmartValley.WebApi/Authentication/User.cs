using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace SmartValley.WebApi.Authentication
{
    public class User : IIdentity
    {
        public string EthereumAddress { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public User(string address, bool isAuthenticated)
        {
            EthereumAddress = address;
            IsAuthenticated = isAuthenticated;
            Name = address;
            AuthenticationType = EcdsaAuthenticationOptions.DefaultScheme;
        }
    }
}
using System.Security.Principal;

namespace SmartValley.WebApi.Authentication
{
    public class Identity : IIdentity
    {
        public string EthereumAddress { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }

        public Identity(string address, bool isAuthenticated)
        {
            EthereumAddress = address;
            IsAuthenticated = isAuthenticated;
            Name = address;
            AuthenticationType = EcdsaAuthenticationOptions.DefaultScheme;
        }
    }
}
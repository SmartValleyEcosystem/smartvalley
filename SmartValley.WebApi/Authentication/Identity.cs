using System.Collections.Generic;

namespace SmartValley.WebApi.Authentication
{
    public class Identity
    {
        public string Address { get; }

        public bool IsAuthenticated { get; }

        public string Token { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }


        public Identity(string address, bool isAuthenticated, string token, IReadOnlyCollection<string> roles)
        {
            Address = address;
            IsAuthenticated = isAuthenticated;
            Token = token;
            Roles = roles;
        }
    }
}
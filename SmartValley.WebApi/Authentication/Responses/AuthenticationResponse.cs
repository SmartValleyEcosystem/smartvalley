using System.Collections.Generic;

namespace SmartValley.WebApi.Authentication.Responses
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }
    }
}
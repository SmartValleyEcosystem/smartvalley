using System.Collections.Generic;
using SmartValley.Domain.Core;

namespace SmartValley.WebApi.Authentication
{
    public class Identity
    {
        public long Id { get; }

        public Address Address { get; }

        public string Email { get; }

        public bool IsAuthenticated { get; }

        public string Token { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }


        public Identity(long id, Address address, string email, bool isAuthenticated, string token, IReadOnlyCollection<string> roles)
        {
            Id = id;
            Address = address;
            Email = email;
            IsAuthenticated = isAuthenticated;
            Token = token;
            Roles = roles;
        }
    }
}
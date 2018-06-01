using System;
using System.Linq;
using System.Security.Claims;
using SmartValley.Domain.Exceptions;

namespace SmartValley.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static long GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
                throw new AppErrorException(ErrorCode.AuthenticationError);
            return Convert.ToInt64(userIdClaim.Value);
        }

        public static long? TryGetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
                return null;
            return Convert.ToInt64(userIdClaim.Value);
        }
    }
}
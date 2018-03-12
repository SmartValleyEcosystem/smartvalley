using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.WebApi.Authentication.Requests;

namespace SmartValley.WebApi.Authentication
{
    public interface IAuthenticationService
    {
        Task<Identity> AuthenticateAsync(AuthenticationRequest request);

        Task RegisterAsync(RegistrationRequest request);

        Task<Identity> RefreshAccessTokenAsync(Address address);

        bool ShouldRefreshToken(string token);

        Task ConfirmEmailAsync(Address address, string token);

        Task ResendEmailAsync(Address address);

        Task ChangeEmailAsync(Address address, string email);

        Task<string> GetEmailBySignatureAsync(Address address, string signature, string signedText);
    }
}
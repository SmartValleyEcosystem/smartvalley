using System.Threading.Tasks;
using SmartValley.WebApi.Authentication.Requests;

namespace SmartValley.WebApi.Authentication
{
    public interface IAuthenticationService
    {
        Task<Identity> AuthenticateAsync(AuthenticationRequest request);

        Task RegisterAsync(RegistrationRequest request);

        Task<Identity> RefreshAccessTokenAsync(string address);

        bool ShouldRefreshToken(string token);

        Task ConfirmEmailAsync(string address, string token);

        Task ResendEmailAsync(string address);

        Task ChangeEmailAsync(string address, string email);

        Task<string> GetEmailBySignatureAsync(string address, string signature, string signedText);
    }
}
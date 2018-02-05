using System.Threading.Tasks;
using SmartValley.WebApi.Authentication.Requests;

namespace SmartValley.WebApi.Authentication
{
    public interface IAuthenticationService
    {
        Task<Identity> AuthenticateAsync(AuthenticationRequest request);
        Task RegisterAsync(RegistrationRequest request);
    }
}
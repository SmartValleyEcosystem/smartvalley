using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Users
{
    public interface IUserService
    {
        Task<User> GetByAddressAsync(string address);
    }
}

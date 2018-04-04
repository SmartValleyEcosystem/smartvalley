using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Users
{
    public interface IUserService
    {
        Task<User> GetByAddressAsync(Address address);

        Task<User> GetByIdAsync(long id);

        Task UpdateAsync(Address address, string name, string about);
    }
}
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Users
{
    public interface IUserService
    {
        Task<User> GetByAddressAsync(string address);

        Task UpdateAsync(string address, string name, string about);
    }
}
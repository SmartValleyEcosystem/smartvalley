using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByAddressAsync(string address);

        Task<User> GetByEmailAsync(string ademaildress);

        Task<int> AddAsync(User user);
    }
}
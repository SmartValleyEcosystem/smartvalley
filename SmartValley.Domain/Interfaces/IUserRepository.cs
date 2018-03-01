using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);

        Task<User> GetByAddressAsync(string address);

        Task<IReadOnlyCollection<User>> GetIdsByAddressesAsync(IReadOnlyCollection<string> addresses);

        Task<User> GetByEmailAsync(string ademaildress);

        Task<int> UpdateWholeAsync(User user);

        Task<IReadOnlyCollection<User>> GetByRoleAsync(RoleType type);

        Task AddRoleAsync(string address, RoleType type);

        Task<bool> HasRoleAsync(string address, RoleType type);

        Task RemoveRoleAsync(string address, RoleType type);

        Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId);

        Task<User> GetByIdAsync(long userId);
    }
}
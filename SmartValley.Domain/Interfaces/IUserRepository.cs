using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);

        Task<IReadOnlyCollection<User>> GetAllAsync();

        Task<int> RemoveAsync(User entity);

        Task<User> GetByAddressAsync(string address);

        Task<User> GetByEmailAsync(string ademaildress);

        Task<int> UpdateWholeAsync(User user);

        Task<IReadOnlyCollection<User>> GetByRoleAsync(RoleType type);

        Task AddRoleAsync(string address, RoleType type);

        Task<bool> HasRoleAsync(string address, RoleType type);

        Task RemoveRoleAsync(string address, RoleType type);

        Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId);
    }
}
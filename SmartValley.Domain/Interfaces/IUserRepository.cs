using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);

        Task<User> GetByAddressAsync(Address address);

        Task<IReadOnlyCollection<User>> GetIdsByAddressesAsync(IReadOnlyCollection<Address> addresses);

        Task<User> GetByEmailAsync(string email);

        Task<User> GetByConfirmedEmailAsync(string email);

        Task<int> UpdateWholeAsync(User user);

        Task<IReadOnlyCollection<User>> GetByRoleAsync(RoleType type);

        Task AddRoleAsync(Address address, RoleType type);

        Task<bool> HasRoleAsync(Address address, RoleType type);

        Task RemoveRoleAsync(Address address, RoleType type);

        Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId);

        Task<User> GetByIdAsync(long userId);
    }
}
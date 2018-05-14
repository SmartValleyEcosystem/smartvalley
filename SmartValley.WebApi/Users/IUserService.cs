using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Users
{
    public interface IUserService
    {
        Task<User> GetByAddressAsync(Address address);

        Task<User> GetByIdAsync(long id);

        Task<int> GetTotalCountAsync();

        Task<IReadOnlyCollection<User>> GetAllAsync(int offset, int count);

        Task UpdateAsync(Address address, string firstName, string secondName);

        Task SetCanCreatePrivateProjectsAsync(Address address, bool canCreatePrivateProjects);
    }
}
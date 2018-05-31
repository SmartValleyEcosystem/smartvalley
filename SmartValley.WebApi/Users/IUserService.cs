using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Users.Requests;

namespace SmartValley.WebApi.Users
{
    public interface IUserService
    {
        Task<User> GetByAddressAsync(Address address);

        Task<User> GetByIdAsync(long id);

        Task<PagingCollection<User>> GetAsync(int offset, int count);

        Task UpdateAsync(Address address, UpdateUserRequest request);

        Task SetCanCreatePrivateProjectsAsync(Address address, bool canCreatePrivateProjects);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Admin
{
    public interface IAdminService
    {
        Task AddAsync(Address address);

        Task DeleteAsync(Address address);

        Task<IReadOnlyCollection<User>> GetAllAsync();

        Task<bool> IsAdminAsync(Address address);
    }
}

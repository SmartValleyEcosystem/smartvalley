using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Admin
{
    public interface IAdminService
    {
        Task AddAsync(string address);

        Task DeleteAsync(string address);

        Task<IReadOnlyCollection<User>> GetAllAsync();

        Task<bool> IsAdminAsync(string address);
    }
}

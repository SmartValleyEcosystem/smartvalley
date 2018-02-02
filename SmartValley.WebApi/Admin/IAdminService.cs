using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.WebApi.Admin
{
    public interface IAdminService
    {
        Task AddAsync(string address);

        Task DeleteAsync(string address);

        Task<IReadOnlyCollection<string>> GetAllAsync();

        Task<bool> IsAdminAsync(string address);
    }
}

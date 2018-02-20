using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertRepository
    {
        Task AddAsync(Expert expert);

        Task<IReadOnlyCollection<Expert>> GetAllAsync();

        Task<IReadOnlyCollection<ExpertDetails>> GetAllDetailsAsync();

        Task<int> RemoveAsync(Expert expert);

        Task<Expert> GetByAddressAsync(string address);

        Task<Expert> GetByEmailAsync(string email);

        Task<int> UpdateWholeAsync(Expert expert);
    }
}

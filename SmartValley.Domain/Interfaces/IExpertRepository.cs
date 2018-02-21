using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertRepository
    {
        Task<IReadOnlyCollection<Expert>> GetAllAsync();

        Task<PagingList<ExpertDetails>> GetAllDetailsAsync(int page, int pageSize);

        Task<int> RemoveAsync(Expert expert);

        Task<Expert> GetByAddressAsync(string address);

        Task<Expert> GetByEmailAsync(string email);

        Task<int> UpdateWholeAsync(Expert expert);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task AddAsync(Expert expert, IReadOnlyCollection<int> areas);

        Task UpdateAsync(Expert expert, IReadOnlyCollection<int> areas);
    }
}
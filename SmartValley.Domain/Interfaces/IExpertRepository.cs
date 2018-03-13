using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertRepository
    {
        Task<PagingList<ExpertDetails>> GetAllDetailsAsync(int page, int pageSize);

        Task<int> RemoveAsync(Expert expert);

        Task<Expert> GetByAddressAsync(Address address);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task AddAsync(long expertId, IReadOnlyCollection<int> areas);

        Task UpdateAsync(Expert expert, IReadOnlyCollection<int> areas);

        Task<Expert> GetAsync(long expertId);

        Task SetAvailabilityAsync(long expertId, bool isAvailable);
    }
}
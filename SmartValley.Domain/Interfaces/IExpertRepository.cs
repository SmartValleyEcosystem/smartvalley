using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertRepository
    {
        Task<IReadOnlyCollection<ExpertDetails>> GetAllDetailsAsync(int offset, int count);

        Task<ExpertDetails> GetDetailsAsync(Address address);

        Task<int> RemoveAsync(Expert expert);

        Task<Expert> GetByAddressAsync(Address address);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task AddAsync(long expertId, IReadOnlyCollection<int> areas);

        Task UpdateAsync(Expert expert, IReadOnlyCollection<int> areas);

        Task<Expert> GetAsync(long expertId);

        Task SetAvailabilityAsync(long expertId, bool isAvailable);

        Task<int> GetTotalCountExpertsAsync();
    }
}
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

        Task AddAsync(Expert expert, IReadOnlyCollection<int> areas);

        Task UpdateAreasAsync(long expertId, IReadOnlyCollection<int> areas);

        Task<Expert> GetAsync(long expertId);

        Task SetAvailabilityAsync(Address address, bool isAvailable);

        Task<int> GetTotalCountExpertsAsync();

        Task SaveChangesAsync();
    }
}
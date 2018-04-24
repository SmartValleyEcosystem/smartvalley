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

        void Remove(Expert expert);

        Task<Expert> GetByAddressAsync(Address address);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        void Add(Expert expert);

        Task<Expert> GetAsync(long expertId);

        Task<int> GetTotalCountExpertsAsync();

        Task SaveChangesAsync();
    }
}
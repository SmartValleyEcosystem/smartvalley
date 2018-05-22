using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertRepository
    {
        Task<PagingCollection<Expert>> GetAsync(int offset, int count);

        void Remove(Expert expert);

        Task<Expert> GetByAddressAsync(Address address);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        void Add(Expert expert);

        Task<Expert> GetByIdAsync(long expertId);

        Task SaveChangesAsync();
    }
}
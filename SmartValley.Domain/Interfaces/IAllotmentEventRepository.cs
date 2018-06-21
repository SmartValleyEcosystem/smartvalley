using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Interfaces
{
    public interface IAllotmentEventRepository
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query, DateTimeOffset now);

        void Add(AllotmentEvent allotmentEvent);

        Task SaveChangesAsync();

        Task<AllotmentEvent> GetByIdAsync(long id);
    }
}
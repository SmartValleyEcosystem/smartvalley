using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Services
{
    public interface IAllotmentEventService
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query);

        Task<AllotmentEvent> GetByIdAsync(long id);

        Task<long> CreateAsync(string name, string tokenContractAddress, int tokenDecimals, string tokenTicker, long projectId, DateTimeOffset? finishDate);

        Task PublishAsync(long allotmentEventId);

        Task UpdateAsync(long id, string name, string tokenContractAddress, int tokenDecimals, string tokenTicker, DateTimeOffset? finishDate);

        Task StartAsync(long allotmentEventId);

        Task SetUpdatingStateAsync(long allotmentEventId, bool isUpdating);
    }
}
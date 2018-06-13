using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Services
{
    public interface IAllotmentEventService
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery queryAllotmentEventsRequest);

        Task<long> CreateAsync(string name, string tokenContractAddress, int totalDecimals, string tokenTicker, long projectId, DateTimeOffset? finishDate);

        Task PublishAsync(long eventId, string transactionHash);
    }
}
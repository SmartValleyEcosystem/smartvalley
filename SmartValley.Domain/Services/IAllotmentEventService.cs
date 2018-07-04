using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IAllotmentEventService
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query);

        Task<long> CreateAsync(string name, string tokenContractAddress, int tokenDecimals, string tokenTicker, long projectId, DateTimeOffset? finishDate);

        Task PublishAsync(long allotmentEventId);

        Task UpdateAsync(long id);
    }
}
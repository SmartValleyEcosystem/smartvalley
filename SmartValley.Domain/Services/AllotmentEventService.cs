using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository)
        {
            _allotmentEventRepository = allotmentEventRepository;
        }

        public async Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery queryAllotmentEventsRequest)
        {
            return await _allotmentEventRepository.QueryAsync(queryAllotmentEventsRequest);
        }

        public async Task<long> CreateAsync(string name, string tokenContractAddress, int totalDecimals, string tokenTicker, long projectId, DateTimeOffset? finishDate)
        {
            var entity = new AllotmentEvent
                         {
                             Name = name,
                             ProjectId = projectId,
                             TokenContractAddress = tokenContractAddress,
                             TokenTicker = tokenTicker,
                             TokenDecimals = totalDecimals,
                             Status = AllotmentEventStatus.Created,
                             FinishDate = finishDate
                         };
            _allotmentEventRepository.Add(entity);
            await _allotmentEventRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task PublishAsync(long eventId, string transactionHash)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(eventId);
            if (allotmentEvent == null)
                throw new AppErrorException(ErrorCode.AllotmentEventNotFound);

            ///TODO SV-1249

            await _allotmentEventRepository.SaveChangesAsync();
        }
    }
}
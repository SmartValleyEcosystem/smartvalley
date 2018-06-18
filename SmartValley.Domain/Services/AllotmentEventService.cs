using System;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;
        private readonly IAllotmentEventsManagerContractClient _allotmentEventsManagerContractClient;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository,
                                     IAllotmentEventsManagerContractClient allotmentEventsManagerContractClient)
        {
            _allotmentEventRepository = allotmentEventRepository;
            _allotmentEventsManagerContractClient = allotmentEventsManagerContractClient;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query)
            => _allotmentEventRepository.QueryAsync(query);

        public Task<AllotmentEvent> GetByIdAsync(long id)
            => _allotmentEventRepository.GetByIdAsync(id);

        public async Task<long> CreateAsync(
            string name,
            string tokenContractAddress,
            int tokenDecimals,
            string tokenTicker,
            long projectId,
            DateTimeOffset? finishDate)
        {
            var entity = new AllotmentEvent
                         {
                             Name = name,
                             ProjectId = projectId,
                             TokenContractAddress = tokenContractAddress,
                             TokenTicker = tokenTicker,
                             TokenDecimals = tokenDecimals,
                             Status = AllotmentEventStatus.Created,
                             FinishDate = finishDate
                         };
            _allotmentEventRepository.Add(entity);

            await _allotmentEventRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(
            long id,
            string name,
            string tokenContractAddress,
            int tokenDecimals,
            string tokenTicker,
            DateTimeOffset? finishDate)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id) ?? throw new AppErrorException(ErrorCode.AllotmentEventNotFound);

            allotmentEvent.Name = name;
            allotmentEvent.TokenContractAddress = tokenContractAddress;
            allotmentEvent.TokenDecimals = tokenDecimals;
            allotmentEvent.TokenTicker = tokenTicker;
            allotmentEvent.FinishDate = finishDate;

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task PublishAsync(long allotmentEventId)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(allotmentEventId);
            allotmentEvent.SetState(AllotmentEventStatus.Published);
            allotmentEvent.EventContractAddress = await _allotmentEventsManagerContractClient.GetAllotmentEventContractAddressAsync(allotmentEventId);

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task StartAsync(long allotmentEventId)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(allotmentEventId);
            allotmentEvent.SetState(AllotmentEventStatus.InProgress);

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task SetUpdatingStateAsync(long allotmentEventId, bool isUpdating)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(allotmentEventId);
            allotmentEvent.IsUpdating = isUpdating;

            await _allotmentEventRepository.SaveChangesAsync();
        }
    }
}
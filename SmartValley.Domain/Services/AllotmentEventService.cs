using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;
        private readonly IEthereumTransactionService _ethereumTransactionService;
        private readonly IAllotmentEventsManagerContractClient _allotmentEventsManagerContractClient;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository,
                                     IEthereumTransactionService ethereumTransactionService,
                                     IAllotmentEventsManagerContractClient allotmentEventsManagerContractClient)
        {
            _allotmentEventRepository = allotmentEventRepository;
            _ethereumTransactionService = ethereumTransactionService;
            _allotmentEventsManagerContractClient = allotmentEventsManagerContractClient;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query)
            => _allotmentEventRepository.QueryAsync(query);

        public Task<AllotmentEvent> GetByIdAsync(long id)
            => _allotmentEventRepository.GetByIdAsync(id);

        public async Task StartPublishingAsync(long id, long userId, string hash)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);
            if (allotmentEvent.Status != AllotmentEventStatus.Created && allotmentEvent.Status != AllotmentEventStatus.Publishing)
                return;

            await _ethereumTransactionService.StartAsync(hash, userId, EthereumTransactionType.PublishAllotmentEvent, id);

            allotmentEvent.Status = AllotmentEventStatus.Publishing;

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task FinishPublishingAsync(long id)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);

            if (allotmentEvent.Status != AllotmentEventStatus.Created && allotmentEvent.Status != AllotmentEventStatus.Publishing)
                return;

            allotmentEvent.Status = AllotmentEventStatus.Published;
            allotmentEvent.EventContractAddress = await _allotmentEventsManagerContractClient.GetAllotmentEventContractAddressAsync(id);

            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task StopPublishingAsync(long id)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);
            var transactions = await _ethereumTransactionService.GetByAllotmentEventIdAsync(id);

            if (allotmentEvent.Status != AllotmentEventStatus.Publishing || transactions.Any(x => x.Status == EthereumTransactionStatus.InProgress))
                return;

            allotmentEvent.Status = AllotmentEventStatus.Created;

            await _allotmentEventRepository.SaveChangesAsync();
        }

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
    }
}
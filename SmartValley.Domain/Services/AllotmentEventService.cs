using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;
        private readonly IClock _clock;
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository, IClock clock, IEthereumTransactionService ethereumTransactionService)
        {
            _allotmentEventRepository = allotmentEventRepository;
            _clock = clock;
            _ethereumTransactionService = ethereumTransactionService;
        }

        public async Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery queryAllotmentEventsRequest)
        {
            return await _allotmentEventRepository.QueryAsync(queryAllotmentEventsRequest);
        }

        public async Task HandlePendingPublishingTransactionAsync(long id, long userId, string hash)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);
            if (allotmentEvent.Status != AllotmentEventStatus.Created && allotmentEvent.Status != AllotmentEventStatus.Publishing)
            {
                return;
            }

            var transaction = new EthereumTransaction(userId, hash, EthereumTransactionType.PublishAllotmentEvent, EthereumTransactionStatus.InProgress, _clock.UtcNow);
            await _ethereumTransactionService.AddAsync(transaction);
            allotmentEvent.Status = AllotmentEventStatus.Publishing;
            await _allotmentEventRepository.SaveChangesAsync();
        }

        public async Task HandleSuccessPublishingTransactionAsync(long id)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);

            if (allotmentEvent.Status == AllotmentEventStatus.Created || allotmentEvent.Status == AllotmentEventStatus.Publishing)
            {
                allotmentEvent.Status = AllotmentEventStatus.Published;
                await _allotmentEventRepository.SaveChangesAsync();
            }
        }

        public async Task HandleFailedPublishingTransactionAsync(long id)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);
            var transactions = await _ethereumTransactionService.GetByAllotmentEventIdAsync(id);

            if (allotmentEvent.Status == AllotmentEventStatus.Publishing &&
                transactions.Count(x => x.Status == EthereumTransactionStatus.InProgress) == 0)
            {
                allotmentEvent.Status = AllotmentEventStatus.Created;
                await _allotmentEventRepository.SaveChangesAsync();
            }
        }
    }
}
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    public class AllotmentEventService : IAllotmentEventService
    {
        private readonly IAllotmentEventRepository _allotmentEventRepository;
        private readonly IEthereumTransactionRepository _ethereumTransactionRepository;

        public AllotmentEventService(IAllotmentEventRepository allotmentEventRepository, IEthereumTransactionRepository ethereumTransactionRepository)
        {
            _allotmentEventRepository = allotmentEventRepository;
            _ethereumTransactionRepository = ethereumTransactionRepository;
        }

        public async Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery queryAllotmentEventsRequest)
        {
            return await _allotmentEventRepository.QueryAsync(queryAllotmentEventsRequest);
        }

        public async Task HandleSuccessPublishingTransaction(long id, AllotmentEventStatus status)
        {
            var allotmentEvent = await _allotmentEventRepository.GetByIdAsync(id);
            allotmentEvent.Status = status;
            await _allotmentEventRepository.SaveChangesAsync();
        }
    }
}
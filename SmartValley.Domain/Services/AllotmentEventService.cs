using System.Threading.Tasks;
using SmartValley.Domain.Core;
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
    }
}
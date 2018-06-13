using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Services
{
    public interface IAllotmentEventService
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery queryAllotmentEventsRequest);

        Task HandleSuccessPublishingTransactionAsync(long id);

        Task HandlePendingPublishingTransactionAsync(long id, long userId, string hash);

        Task HandleFailedPublishingTransactionAsync(long id);
    }
}
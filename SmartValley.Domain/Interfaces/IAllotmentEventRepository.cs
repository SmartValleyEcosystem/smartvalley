using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Interfaces
{
    public interface IAllotmentEventRepository
    {
        Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query);
    }
}
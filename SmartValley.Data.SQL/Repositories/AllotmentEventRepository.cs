using System.Linq;
using System.Threading.Tasks;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class AllotmentEventRepository : IAllotmentEventRepository
    {
        private readonly IReadOnlyDataContext _readOnlyDataContext;

        public AllotmentEventRepository(IReadOnlyDataContext readOnlyDataContext)
        {
            _readOnlyDataContext = readOnlyDataContext;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query)
        {
            var queryable = _readOnlyDataContext.AllotmentEvents
                                                .Where(e => query.AllotmentEventStatuses.Count == 0 || query.AllotmentEventStatuses.Contains(e.Status));

            return queryable.GetPageAsync(query.Offset, query.Count);
        }
    }
}
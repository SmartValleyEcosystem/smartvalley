using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEditableDataContext _editDataContext;

        public AllotmentEventRepository(IReadOnlyDataContext readOnlyDataContext, IEditableDataContext editDataContext)
        {
            _readOnlyDataContext = readOnlyDataContext;
            _editDataContext = editDataContext;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query)
        {
            var queryable = _readOnlyDataContext.AllotmentEvents
                                                .Where(e => query.AllotmentEventStatuses.Count == 0 || query.AllotmentEventStatuses.Contains(e.Status));

            return queryable.GetPageAsync(query.Offset, query.Count);
        }

        public async Task<AllotmentEvent> GetByIdAsync(long id)
        {
            return await _readOnlyDataContext.AllotmentEvents.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _editDataContext.SaveAsync();
        }
    }
}
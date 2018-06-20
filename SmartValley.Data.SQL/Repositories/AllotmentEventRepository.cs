using System;
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
        private readonly IEditableDataContext _editContext;

        public AllotmentEventRepository(IReadOnlyDataContext readOnlyDataContext, IEditableDataContext editableDataContext)
        {
            _readOnlyDataContext = readOnlyDataContext;
            _editContext = editableDataContext;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query, DateTimeOffset now)
        {
            var queryable = _readOnlyDataContext.AllotmentEvents
                                                .Where(e => query.AllotmentEventStatuses.Count == 0 
                                                            || query.AllotmentEventStatuses.Contains(e.Status) && !(e.Status == AllotmentEventStatus.InProgress && e.FinishDate <= now)
                                                            || query.AllotmentEventStatuses.Contains(AllotmentEventStatus.Finished) && e.Status == AllotmentEventStatus.InProgress && e.FinishDate <= now);

            return queryable.GetPageAsync(query.Offset, query.Count);
        }

        public void Add(AllotmentEvent allotmentEvent)
            => _editContext.AllotmentEvents.Add(allotmentEvent);

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        public Task<AllotmentEvent> GetByIdAsync(long id)
            => _editContext.AllotmentEvents.FirstOrDefaultAsync(i => i.Id == id);
    }
}
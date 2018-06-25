using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class AllotmentEventRepository : IAllotmentEventRepository
    {
        private readonly IEditableDataContext _editContext;

        public AllotmentEventRepository(IEditableDataContext editableDataContext)
        {
            _editContext = editableDataContext;
        }

        public Task<PagingCollection<AllotmentEvent>> QueryAsync(AllotmentEventsQuery query, DateTimeOffset now)
        {
            var queryable = Entities()
                .Where(e => query.AllotmentEventStatuses.Count == 0
                            || query.AllotmentEventStatuses.Contains(e.Status) && !(e.Status == AllotmentEventStatus.InProgress && e.FinishDate <= now)
                            || query.AllotmentEventStatuses.Contains(AllotmentEventStatus.Finished) && e.Status == AllotmentEventStatus.InProgress && e.FinishDate <= now);

            return queryable.GetPageAsync(query.Offset, query.Count);
        }

        public void Add(AllotmentEvent allotmentEvent)
            => _editContext.AllotmentEvents.Add(allotmentEvent);

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        public void Remove(AllotmentEvent entity)
            => _editContext.AllotmentEvents.Remove(entity);

        public Task<AllotmentEvent> GetByIdAsync(long id)
            => Entities().FirstOrDefaultAsync(i => i.Id == id);

        private IQueryable<AllotmentEvent> Entities()
            => _editContext.AllotmentEvents
                           .Include(x => x.Participants);
    }
}
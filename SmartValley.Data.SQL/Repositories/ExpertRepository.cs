using System.Collections.Generic;
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
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExpertRepository : IExpertRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ExpertRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public Task<Expert> GetByAddressAsync(Address address)
        {
            return Entities().FirstOrDefaultAsync(x => x.User.Address == address);
        }

        public Task<Expert> GetByIdAsync(long expertId)
        {
            return Entities().FirstOrDefaultAsync(e => e.UserId == expertId);
        }

        public async Task<IReadOnlyCollection<Area>> GetAreasAsync()
            => await _readContext.Areas.ToArrayAsync();

        public void Add(Expert expert)
        {
            _editContext.Experts.Add(expert);
        }

        public void Remove(Expert expert)
        {
            _editContext.Experts.Remove(expert);
        }

        public async Task SaveChangesAsync()
        {
            await _editContext.SaveAsync();
        }

        public Task<PagingCollection<Expert>> GetAsync(ExpertsQuery query)
        {
            return Entities()
                   .Where(x => !query.IsInHouse.HasValue || query.IsInHouse.Value == x.IsInHouse)
                   .GetPageAsync(query.Offset, query.Count);
        }

        private IQueryable<Expert> Entities() => _editContext.Experts
                                                             .Include(i => i.User)
                                                             .Include(i => i.ExpertAreas).ThenInclude(a => a.Area);
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IEditableDataContext _editContext;
        private readonly IReadOnlyDataContext _readContext;

        public FeedbackRepository(IEditableDataContext editContext, IReadOnlyDataContext readContext)
        {
            _editContext = editContext;
            _readContext = readContext;
        }

        public void Add(Feedback feedback)
        {
            _editContext.Feedbacks.Add(feedback);
        }

        public async Task<IReadOnlyCollection<Feedback>> GetAsync(int offset, int count)
        {
            return await _readContext.Feedbacks
                                     .Skip(offset)
                                     .Take(count)
                                     .ToArrayAsync();
        }

        public Task<int> GetTotalCountAsync()
            => _readContext.Feedbacks.CountAsync();

        public Task SaveChangesAsync()
        {
            return _editContext.SaveAsync();
        }
    }
}
using System.Threading.Tasks;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain.Core;
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

        public Task<PagingCollection<Feedback>> GetAsync(int offset, int count)
            => _readContext.Feedbacks.GetPageAsync(offset, count);

        public Task SaveChangesAsync()
        {
            return _editContext.SaveAsync();
        }
    }
}
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IFeedbackRepository
    {
        void Add(Feedback feedback);

        Task<PagingCollection<Feedback>> GetAsync(int offset, int count);

        Task SaveChangesAsync();
    }
}
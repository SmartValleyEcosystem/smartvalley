using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IFeedbackRepository
    {
        void Add(Feedback feedback);

        Task<IReadOnlyCollection<Feedback>> GetAsync(int offset, int count);

        Task<int> GetTotalCountAsync();

        Task SaveChangesAsync();
    }
}
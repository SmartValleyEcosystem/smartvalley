using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Feedbacks.Requests;

namespace SmartValley.WebApi.Feedbacks
{
    public interface IFeedbackService
    {
        Task AddAsync(CreateFeedbackRequest request);

        Task<IReadOnlyCollection<Feedback>> GetAsync(int offset, int count);

        Task<int> GetTotalCountAsync();
    }
}

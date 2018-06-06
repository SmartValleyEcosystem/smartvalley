using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Feedbacks.Requests;

namespace SmartValley.WebApi.Feedbacks
{
    public interface IFeedbackService
    {
        Task AddAsync(CreateFeedbackRequest request);

        Task<PagingCollection<Feedback>> GetAsync(int offset, int count);
    }
}

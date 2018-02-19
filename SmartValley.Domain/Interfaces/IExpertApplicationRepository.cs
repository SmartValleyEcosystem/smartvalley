using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertApplicationRepository
    {
        Task<IReadOnlyCollection<ExpertApplication>> GetAllByStatusAsync(ExpertApplicationStatus status);

        Task<ExpertApplicationDetails> GetDetailsByIdAsync(long id);

        Task<int> AddAsync(ExpertApplication expertApplication, IReadOnlyCollection<int> areas);

        Task<bool> IsAppliedAsync(string address);

        Task<int> UpdateWholeAsync(ExpertApplication expertApplication);

        Task SetAcceptedAsync(ExpertApplicationDetails applicationDetails, List<int> areas);

        Task SetRejectedAsync(ExpertApplicationDetails applicationDetails);
    }
}
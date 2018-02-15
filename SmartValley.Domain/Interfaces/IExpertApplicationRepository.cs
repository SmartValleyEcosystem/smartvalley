using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertApplicationRepository
    {
        Task<int> AddAsync(ExpertApplication expertApplication, IReadOnlyCollection<int> areas);

        Task<bool> IsAppliedAsync(string address);

        Task<bool> IsConfirmedAsync(string address);

        Task<int> UpdateWholeAsync(ExpertApplication expertApplication);
    }
}
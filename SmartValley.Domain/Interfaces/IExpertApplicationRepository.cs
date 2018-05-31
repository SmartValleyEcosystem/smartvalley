using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertApplicationRepository
    {
        Task<IReadOnlyCollection<ExpertApplication>> GetAllByStatusAsync(ExpertApplicationStatus status);

        Task<ExpertApplication> GetByIdAsync(long id);

        Task<ExpertApplication> GetByUserIdAsync(long userId);

        void Add(ExpertApplication expertApplication);

        Task SaveChangesAsync();
    }
}
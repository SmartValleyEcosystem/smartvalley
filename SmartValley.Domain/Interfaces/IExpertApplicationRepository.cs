using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IExpertApplicationRepository
    {
        Task<IReadOnlyCollection<ExpertApplication>> GetAllByStatusAsync(ExpertApplicationStatus status);

        Task<ExpertApplicationDetails> GetDetailsByIdAsync(long id);

        void Add(ExpertApplication expertApplication, IReadOnlyCollection<int> areas);

        void SetAccepted(ExpertApplicationDetails applicationDetails, List<int> areas);

        void SetRejected(ExpertApplicationDetails applicationDetails);

        Task SaveChangesAsync();

        Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address);
    }
}
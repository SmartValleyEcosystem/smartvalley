using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public interface IExpertService
    {
        Task CreateApplicationAsync(CreateExpertApplicationRequest request, long userId, AzureFile cv, AzureFile scan, AzureFile photo);

        Task<ExpertApplicationDetails> GetApplicationByIdAsync(long id);

        Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync();

        Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas);

        Task RejectApplicationAsync(long id);

        Task AddAsync(ExpertRequest request);

        Task UpdateAsync(Address address, ExpertUpdateRequest request);

        Task DeleteAsync(Address address);

        Task<IReadOnlyCollection<ExpertDetails>> GetAllExpertsDetailsAsync(int offset, int count);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address);

        Task<Expert> GetAsync(long expertId);

        Task<ExpertDetails> GetDetailsAsync(Address address);

        Task SetAvailabilityAsync(long expertId, bool isAvailable);

        Task<int> GetTotalCountExpertsAsync();
    }
}
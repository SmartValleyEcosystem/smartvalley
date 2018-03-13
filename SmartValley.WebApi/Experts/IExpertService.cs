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
        Task CreateApplicationAsync(CreateExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo);

        Task<ExpertApplicationDetails> GetApplicationByIdAsync(long id);

        Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync();

        Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas);

        Task RejectApplicationAsync(long id);

        Task AddAsync(ExpertRequest request);

        Task UpdateAsync(UpdateExpertRequest request);

        Task DeleteAsync(Address address);

        Task<PagingList<ExpertDetails>> GetAllExpertsDetailsAsync(int page, int pageSize);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address);

        Task<Expert> GetAsync(long expertId);

        Task SwitchAvailabilityAsync(long expertId);
    }
}
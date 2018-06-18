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

        Task<ExpertApplication> GetApplicationByIdAsync(long id);

        Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync();

        Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas);

        Task RejectApplicationAsync(long id);

        Task AddAsync(ExpertRequest request);

        Task DeleteAsync(Address address);

        Task<PagingCollection<Expert>> GetAsync(ExpertsQuery query);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address);

        Task<Expert> GetByIdAsync(long expertId);

        Task<Expert> GetByAddressAsync(Address address);

        Task SetAvailabilityAsync(Address address, bool isAvailable);

        Task SetInHouseAsync(Address address, bool isInHouse);
    }
}
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

        Task UpdateAsync(ExpertRequest request);

        Task DeleteAsync(string address);

        Task<PagingList<ExpertDetails>> GetAllExpertsDetailsAsync(int page, int pageSize);

        Task<bool> IsExpertAsync(string address);

        Task<IReadOnlyCollection<Area>> GetAreasAsync();

        Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(string address);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public interface IExpertService
    {
        Task CreateApplicationAsync(CreateExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo);

        Task<ExpertApplicationDetails> GetApplicationByIdAsync(long id);

        Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync();

        Task<bool> IsAppliedAsync(string address);

        Task<bool> IsConfirmedAsync(string address);

        Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas);

        Task RejectApplicationAsync(long id);

        Task AddAsync(string address);

        Task DeleteAsync(string address);

        Task<IReadOnlyCollection<ExpertDetails>> GetAllExpertsDetailsAsync();

        Task<bool> IsExpertAsync(string address);
    }
}
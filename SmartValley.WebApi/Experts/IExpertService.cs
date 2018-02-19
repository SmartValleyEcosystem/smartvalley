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
        Task CreateApplicationAsync(ExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo);

        Task<bool> IsAppliedAsync(string address);

        Task<bool> IsConfirmedAsync(string address);

        Task AddAsync(string address);

        Task DeleteAsync(string address);

        Task<IReadOnlyCollection<ExpertDetails>> GetAllExpertsDetailsAsync();

        Task<bool> IsExpertAsync(string address);
    }
}
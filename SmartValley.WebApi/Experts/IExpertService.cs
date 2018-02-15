using System.IO;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public interface IExpertService
    {
        Task CreateApplicationAsync(ExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo);

        Task<bool> IsAppliedAsync(string address);

        Task<bool> IsConfirmedAsync(string address);
    }
}
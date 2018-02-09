using System.Threading.Tasks;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public interface IExpertService
    {
        Task CreateApplicationAsync(ExpertApplicationRequest request);

        Task<bool> IsAppliedAsync(string address);

        Task<bool> IsConfirmedAsync(string address);
    }
}
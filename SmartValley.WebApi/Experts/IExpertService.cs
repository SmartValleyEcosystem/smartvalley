using System.Threading.Tasks;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public interface IExpertService
    {
        Task CreateApplicationAsync(ExpertApplicationRequest request);
    }
}
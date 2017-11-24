using System.Threading.Tasks;

namespace SmartValley.WebApi.Applications
{
    public interface IApplicationService
    {
        Task CreateApplicationAsync(ApplicationRequest applicationRequest);
        Task<ApplicationResponse> GetByProjectIdAsync(long projectId);
    }
}
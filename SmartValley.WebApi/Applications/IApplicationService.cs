using System.Threading.Tasks;

namespace SmartValley.WebApi.Applications
{
    public interface IApplicationService
    {
        Task CreateAsync(ApplicationRequest applicationRequest);
    }
}
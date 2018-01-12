using System.Threading.Tasks;
using SmartValley.WebApi.Applications.Requests;

namespace SmartValley.WebApi.Applications
{
    public interface IApplicationService
    {
        Task CreateAsync(ApplicationRequest applicationRequest);
    }
}
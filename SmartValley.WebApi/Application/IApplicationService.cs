using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Application
{
    public interface IApplicationService
    {
        Task CreateApplicationAsync(ApplicationRequest applicationRequest);
    }
}
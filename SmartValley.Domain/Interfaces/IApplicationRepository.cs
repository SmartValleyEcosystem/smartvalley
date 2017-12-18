using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IApplicationRepository
    {
        Task<int> AddAsync(Application application);

        Task<Application> GetByProjectIdAsync(long projectId);
    }
}
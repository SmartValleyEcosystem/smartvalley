using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IApplicationRepository
    {
        Task<int> AddAsync(Application application);

        Task<Application> GetByProjectIdAsync(long projectId);

        Task<IReadOnlyCollection<Category>> GetCategoriesAsync();

        Task<IReadOnlyCollection<Stage>> GetStagesAsync();
    }
}
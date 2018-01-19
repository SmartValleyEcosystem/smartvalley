using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetails> GetDetailsAsync(long projectId);

        Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync();

        Task<bool> IsAuthorizedToSeeEstimatesAsync(string account, long projectId);
    }
}
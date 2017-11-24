using System.Threading.Tasks;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetailsResponse> GetProjectDetailsByIdAsync(long projectId);
    }
}
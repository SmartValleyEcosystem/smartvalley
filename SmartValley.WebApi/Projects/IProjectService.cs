using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<Project> GetProjectByIdAsync(long projectId);
        Task<ProjectDetailsResponse> GetProjectDetailsByIdAsync(long projectId);
    }
}
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects.Requests;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<PagingCollection<Project>> GetAsync(ProjectsQuery query);

        Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectId);

        Task<bool> IsAuthorizedToEditProjectAsync(long projectId, long userId);

        Task UpdateTeamMemberPhotoAsync(long projectId, long projectTeamMemberId, AzureFile photo);

        Task<Project> CreateAsync(long userId, CreateProjectRequest request);

        Task<Project> UpdateAsync(long projectId, UpdateProjectRequest request);

        Task DeleteAsync(long projectId);

        Task UpdateImageAsync(long projectId, AzureFile image);

        Task<Project> GetByIdAsync(long projectId);

        Task<Project> GetByAuthorIdAsync(long authorId);

        Task DeleteTeamMemberPhotoAsync(long projectId, long projectTeamMemberId);

        Task DeleteProjectImageAsync(long projectId);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects.Requests;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetails> GetDetailsAsync(long projectId);

        Task<IReadOnlyCollection<ProjectDetails>> QueryAsync(ProjectsQuery projectsQuery);

        Task<int> GetQueryTotalCountAsync(ProjectsQuery projectsQuery);

        Task<bool> IsAuthorizedToSeeEstimatesAsync(long userId, long projectId);

        Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectTeamMemberId);

        Task<bool> IsAuthorizedToEditProjectAsync(long projectId, long userId);

        Task UpdateTeamMemberPhotoAsync(long projectTeamMemberId, AzureFile photo);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName);

        Task CreateAsync(long userId, CreateProjectRequest request);

        Task UpdateAsync(long projectId, UpdateProjectRequest request);

        Task DeleteAsync(long projectId);

        Task UpdateImageAsync(long projectId, AzureFile image);

        Task<Project> GetAsync(long projectId);

        Task<IReadOnlyCollection<ProjectTeamMember>> GetTeamAsync(long projectId);

        Task<ProjectDetails> GetByAuthorIdAsync(long authorId);
    }
}
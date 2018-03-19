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

        Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery);

        Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery);

        Task<bool> IsAuthorizedToSeeEstimatesAsync(long userId, long projectId);

        Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectTeamMemberId);

        Task UpdateTeamMemberPhotoAsync(long projectTeamMemberId, AzureFile photo);

        Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName);

        Task CreateAsync(long userId, CreateProjectRequest request);

        Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorIdAsync(long authorId);
    }
}
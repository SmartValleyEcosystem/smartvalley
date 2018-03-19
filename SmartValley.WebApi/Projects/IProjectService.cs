using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects.Requests;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetails> GetDetailsAsync(long projectId);

        Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery);

        Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery);

        Task<bool> IsAuthorizedToSeeEstimatesAsync(Address account, long projectId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorAsync(Address authorAddress);

        Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName);

        Task CreateAsync(CreateProjectRequest request);
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(long id);

        Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery);

        Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery);

        Task<int> AddAsync(Project project);

        Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId);

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName);

        Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorIdAsync(long authorId);

        Task<int> RemoveAsync(Project project);

        Task<int> UpdateAsync(Project project);

        Task<int> UpdateAsync(Project project, params Expression<Func<Project, object>>[] properties);
    }
}
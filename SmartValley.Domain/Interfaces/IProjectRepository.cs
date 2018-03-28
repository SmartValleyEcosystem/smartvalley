using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(long id);

        Task<IReadOnlyCollection<ProjectDetails>> QueryAsync(ProjectsQuery projectsQuery);

        Task<int> GetQueryTotalCountAsync(ProjectsQuery projectsQuery);

        Task<int> AddAsync(Project project);

        Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId);

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName);

        Task<ProjectDetails> GetByAuthorIdAsync(long authorId);

        Task<int> RemoveAsync(Project project);

        Task<int> UpdateAsync(Project project, params Expression<Func<Project, object>>[] properties);

        Task<int> UpdateWholeAsync(Project project);
    }
}
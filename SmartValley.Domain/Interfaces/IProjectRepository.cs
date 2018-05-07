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

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName);

        Task<ProjectDetails> GetByAuthorIdAsync(long authorId);

        void Delete(Project project);

        Task<int> UpdateAsync(Project project, params Expression<Func<Project, object>>[] properties);

        Task<int> UpdateWholeAsync(Project project);

        Task<Project> GetAsync(long projectId);

        Task SaveChangesAsync();
    }
}
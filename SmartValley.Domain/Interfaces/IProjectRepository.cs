using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<IReadOnlyCollection<Project>> QueryAsync(ProjectsQuery projectsQuery);

        Task<int> GetQueryTotalCountAsync(ProjectsQuery projectsQuery);

        void Add(Project project);

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<IReadOnlyCollection<Project>> GetAllByNameAsync(string projectName);

        Task<Project> GetByAuthorIdAsync(long authorId);

        void Delete(Project project);

        Task<Project> GetAsync(long projectId);

        Task SaveChangesAsync();
    }
}
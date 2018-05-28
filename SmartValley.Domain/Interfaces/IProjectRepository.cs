using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<PagingCollection<Project>> GetAsync(ProjectsQuery query);

        void Add(Project project);

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<Project> GetByAuthorIdAsync(long authorId);

        void Remove(Project entity);

        Task<Project> GetByIdAsync(long projectId);

        Task SaveChangesAsync();
    }
}
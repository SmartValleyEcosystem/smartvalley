using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(long id);

        Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(int page, int pageSize);

        Task<int> AddAsync(Project project);

        Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorAsync(Address authorAddress);

        Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId);

        Task<Project> GetByExternalIdAsync(Guid externalId);

        Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);

        Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName);
    }
}
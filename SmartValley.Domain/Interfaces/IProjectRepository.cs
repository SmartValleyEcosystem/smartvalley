using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(long id);

        Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync();

        Task<int> AddAsync(Project project);

        Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(string authorAddress);

        Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(string expertAddress, ExpertiseArea area);

        Task<Project> GetByExternalIdAsync(Guid externalId);
    }
}
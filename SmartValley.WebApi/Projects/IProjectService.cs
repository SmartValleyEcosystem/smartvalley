using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetails> GetDetailsAsync(long projectId);

        Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync();

        Task<bool> IsAuthorizedToSeeEstimatesAsync(string account, long projectId);

        Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(string authorAddress);

        Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(ExpertiseAreaType expertiseAreaType, string expertAddress);

        Task<IReadOnlyCollection<Project>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects
{
    public interface IProjectService
    {
        Task<ProjectDetails> GetDetailsAsync(long projectId);

        Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync();

        Task<bool> IsAuthorizedToSeeEstimatesAsync(Address account, long projectId);

        Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(Address authorAddress);

        Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(AreaType areaType, Address expertAddress);

        Task<IReadOnlyCollection<Project>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds);
    }
}
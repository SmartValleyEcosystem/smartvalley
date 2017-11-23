using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using SmartValley.Domain.Entities;
using Project = SmartValley.Domain.Entities.Project;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IReadOnlyCollection<Project>> GetProjectsForScoringByCategoryAsync(ScoringCategory category);
        Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address);
    }
}
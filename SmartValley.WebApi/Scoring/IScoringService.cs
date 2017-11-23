using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using SmartValley.Domain.Entities;
using Project = SmartValley.Domain.Entities.Project;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IEnumerable<Project>> GetProjectsForScoringByCategory(ScoringCategory requestScoringCategory);
        Task<IEnumerable<Project>> GetProjectsByAddress(string address);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Estimates;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IReadOnlyCollection<Project>> GetProjectsForScoringByCategoryAsync(Category category);
        Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address);
    }
}
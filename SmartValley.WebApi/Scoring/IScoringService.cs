using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Estimates;
using ExpertiseArea = SmartValley.Domain.Entities.ExpertiseArea;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IReadOnlyCollection<Project>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress);

        Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address);
    }
}
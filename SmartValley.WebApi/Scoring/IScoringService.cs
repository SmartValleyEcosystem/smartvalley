using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IReadOnlyCollection<Project>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress);

        Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address);
    }
}
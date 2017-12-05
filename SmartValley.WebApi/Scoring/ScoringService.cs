using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEstimateRepository _estimateRepository;

        public ScoringService(
            IProjectRepository projectRepository,
            IEstimateRepository estimateRepository)
        {
            _projectRepository = projectRepository;
            _estimateRepository = estimateRepository;
        }

        public async Task<IReadOnlyCollection<Project>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress)
        {
            var projectsEstimatedByExpert = await _estimateRepository.GetProjectsEstimatedByExpertAsync(expertAddress, expertiseArea);
            var notScoredProjects = await _projectRepository.GetAllByExpertiseAreaAsync(expertiseArea);

            return notScoredProjects.Where(p => !projectsEstimatedByExpert.Contains(p.Id)).ToArray();
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address)
            => _projectRepository.GetAllByAuthorAddressAsync(address);
    }
}
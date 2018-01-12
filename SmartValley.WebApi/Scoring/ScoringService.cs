using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;

        public ScoringService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress)
            => _projectRepository.GetForScoringAsync(expertAddress, expertiseArea);

        public Task<IReadOnlyCollection<ProjectScoring>> GetProjectsByAuthorAsync(string authorAddress)
            => _projectRepository.GetByAuthorAsync(authorAddress);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Estimates;

namespace SmartValley.WebApi.Scoring
{
    public class ScoringService : IScoringService
    {
        private readonly IProjectRepository _projectRepository;

        public ScoringService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsForScoringByCategoryAsync(Category category)
        {
            return _projectRepository.GetAllByCategoryAsync(category.ToDomain());
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsByAuthorAddressAsync(string address)
        {
            return _projectRepository.GetAllByAuthorAddressAsync(address);
        }
    }
}
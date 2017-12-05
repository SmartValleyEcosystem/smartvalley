using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(long id);

        Task<IReadOnlyCollection<Project>> GetAllScoredAsync();

        Task<int> AddAsync(Project project);

        Task<int> UpdateWholeAsync(Project project);

        Task<IReadOnlyCollection<Project>> GetAllByAuthorAddressAsync(string address);

        Task<IReadOnlyCollection<Project>> GetAllByExpertiseAreaAsync(ExpertiseArea expertiseArea);
    }
}
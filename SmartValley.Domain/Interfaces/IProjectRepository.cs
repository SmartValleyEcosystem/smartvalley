using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<IReadOnlyCollection<Project>> GetAllScoredAsync();

        Task<int> AddAsync(Project project);
        Task<List<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetAllByAuthorAddressAsync(string address);
    }
}
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringRepository
    {
        Task<int> AddAsync(Scoring scoring);
        
        Task<int> UpdateWholeAsync(Scoring scoring);

        Task<Scoring> GetByProjectIdAsync(long projectId);
    }
}
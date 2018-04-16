using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringApplicationRepository
    {
        Task<ScoringApplication> GetByProjectIdAsync(long projectId);

        Task<ScoringApplication> GetByIdAsync(long id);

        Task SaveChangesAsync();

        void Add(ScoringApplication scoringApplication);
    }
}
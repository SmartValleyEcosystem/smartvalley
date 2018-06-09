using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringApplicationRepository
    {
        Task<ScoringApplication> GetByProjectIdAsync(long projectId);

        Task<IReadOnlyCollection<ScoringApplication>> GetByProjectIdsAsync(IReadOnlyCollection<long> projectIds);

        Task<ScoringApplication> GetByIdAsync(long id);

        Task SaveChangesAsync();

        void Add(ScoringApplication scoringApplication);
    }
}
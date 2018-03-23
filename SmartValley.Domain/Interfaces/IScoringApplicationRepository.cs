using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringApplicationRepository
    {
        Task<ScoringApplication> GetByProjectIdAsync(long projectId);
        Task SaveAsync(ScoringApplication scoringApplication);
    }
}
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IScoringService
    {
        Task<long> StartAsync(long projectId);

        Task<Scoring> GetByIdAsync(long scoringId);
    }
}
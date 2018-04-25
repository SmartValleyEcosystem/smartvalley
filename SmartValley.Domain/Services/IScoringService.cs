using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IScoringService
    {
        Task<long> StartAsync(long projectId, IDictionary<AreaType, int> areas);

        Task<Scoring> GetAsync(long scoringId);
    }
}
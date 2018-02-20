using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Repositories
{
    public interface IScoringOffersRepository
    {
        Task AddAsync(IReadOnlyCollection<ScoringOffer> offers);

        Task AcceptAsync(long scoringId, long expertId, AreaType area);

        Task RejectAsync(long scoringId, long expertId, AreaType area);

        Task<IReadOnlyCollection<ScoringOffer>> GetByScoringAsync(long projectId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Repositories
{
    public interface IScoringOffersRepository
    {
        Task AddAsync(IReadOnlyCollection<ScoringOffer> offers);

        Task AcceptAsync(long scoringId, long expertId, AreaType area);

        Task RejectAsync(long scoringId, long expertId, AreaType area);

        Task FinishAsync(long scoringId, long expertId, AreaType area);

        Task<bool> IsAcceptedAsync(long scoringId, long expertId, AreaType area);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllPendingByExpertAsync(string expertAddress);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllAcceptedByExpertAsync(string expertAddress);
    }
}
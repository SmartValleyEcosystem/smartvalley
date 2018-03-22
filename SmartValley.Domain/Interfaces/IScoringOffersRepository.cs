using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringOffersRepository
    {
        Task<ScoringOffer> GetAsync(long projectId, AreaType areaType, long expertId);

        Task AddAsync(IReadOnlyCollection<ScoringOffer> offers);

        Task AcceptAsync(long scoringId, long expertId, AreaType area);

        Task RejectAsync(long scoringId, long expertId, AreaType area);

        Task FinishAsync(long scoringId, long expertId, AreaType area);

        Task<bool> IsAcceptedAsync(long scoringId, long expertId, AreaType area);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllPendingByExpertAsync(long expertId, DateTimeOffset now);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllAcceptedByExpertAsync(long expertId, DateTimeOffset now);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetExpertOffersHistoryAsync(long expertId, DateTimeOffset now);

        Task<IReadOnlyCollection<ScoringOffer>> GetByScoringIdAsync(long scoringId);
    }
}
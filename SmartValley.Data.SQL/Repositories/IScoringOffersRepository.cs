using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Repositories
{
    public interface IScoringOffersRepository
    {
        Task<ScoringOffer> GetAsync(long projectId, AreaType areaType, long expertId);

        Task AddAsync(IReadOnlyCollection<ScoringOffer> offers);

        Task AcceptAsync(long scoringId, long expertId, AreaType area);

        Task RejectAsync(long scoringId, long expertId, AreaType area);

        Task FinishAsync(long scoringId, long expertId, AreaType area);

        Task<bool> IsAcceptedAsync(long scoringId, long expertId, AreaType area);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllPendingByExpertAsync(long expertId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllAcceptedByExpertAsync(long expertId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetExpertOffersHistoryAsync(long expertId, DateTimeOffset now);
    }
}
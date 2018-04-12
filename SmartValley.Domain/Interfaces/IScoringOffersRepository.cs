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

        Task<IReadOnlyCollection<ScoringOfferDetails>> QueryAsync(OffersQuery query, DateTimeOffset now);

        Task<int> GetQueryCountAsync(OffersQuery query, DateTimeOffset now);
    }
}
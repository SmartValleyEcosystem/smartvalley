using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringOffersRepository
    {
        Task<IReadOnlyCollection<ScoringOfferDetails>> QueryAsync(OffersQuery query, DateTimeOffset now);

        Task<int> GetQueryCountAsync(OffersQuery query, DateTimeOffset now);
    }
}
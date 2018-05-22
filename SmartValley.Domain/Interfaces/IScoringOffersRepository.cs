using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringOffersRepository
    {
        Task<PagingCollection<ScoringOfferDetails>> GetAsync(OffersQuery query, DateTimeOffset now);
    }
}
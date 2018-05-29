using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringOffersManagerContractClient
    {
        Task<IReadOnlyCollection<ScoringOfferInfo>> GetOffersAsync(Guid projectExternalId);
    }
}
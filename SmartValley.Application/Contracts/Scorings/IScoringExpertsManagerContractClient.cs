using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Application.Contracts.Scorings
{
    public interface IScoringExpertsManagerContractClient
    {
        Task<IReadOnlyCollection<ScoringOfferInfo>> GetOffersAsync(Guid projectExternalId);
    }
}
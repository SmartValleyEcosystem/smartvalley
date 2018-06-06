using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringsRegistryContractClient
    {
        Task<Address> GetScoringAddressAsync(Guid projectExternalId);

        Task<IReadOnlyCollection<AreaExpertsCount>> GetRequiredExpertsCountsAsync(Guid projectExternalId);
    }
}
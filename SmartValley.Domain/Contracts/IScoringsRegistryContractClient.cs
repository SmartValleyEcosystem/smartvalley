using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringsRegistryContractClient
    {
        Task<Address> GetScoringAddressAsync(Guid projectExternalId);
    }
}